
using Libreria.Entidades;
using Libreria.Interfaces;
using Newtonsoft.Json;
using PemUtils;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Servicios
{
    public class ServicioSeguridad : IServicioSeguridad
    {
        private ILogger log;
        private string fichero;
        private static RSAParameters claveRSA = new RSAParameters();

        public ServicioSeguridad(string fichero, ILogger log)
        {
            this.fichero = fichero;
            this.log = log;
        }

        public static void LimpiaClaveRSA() => claveRSA = new RSAParameters();

        private byte[] CifrarRSA(byte[] bytesPlanos)
        {
            byte[] bytesCifrados = null;
            
            try
            {
                using (var rsa = GetProveedorRSA())
                    bytesCifrados = rsa.Encrypt(bytesPlanos, RSAEncryptionPadding.Pkcs1);
            }
            catch(Exception ex)
            {
                //log.Error($"ERR SERVICIOSEGURIDAD (CifrarRSA) - {ex.Message}");
                Console.WriteLine($"ERR SERVICIOSEGURIDAD (CifrarRSA) - {ex.Message}");
            }

            return bytesCifrados;
        }

        private byte[] DescifrarRSA(byte[] bytesCifrados)
        {
            byte[] bytesDescifrados = null;

            try
            {
                using (var rsa = GetProveedorRSA())
                    bytesDescifrados = rsa.Decrypt(bytesCifrados, RSAEncryptionPadding.Pkcs1);
            }
            catch(Exception ex)
            {
                log.Error($"ERR SERVICIOSEGURIDAD (DescifrarRSA) - {ex.Message}");
            }

            return bytesDescifrados;
        }
        
        private RSA GetProveedorRSA()
        {
            var rsa = RSA.Create();

            if (claveRSA.Equals(new RSAParameters()))
            {
                try
                {
                    using (var ficheroClave = File.OpenRead(fichero))
                    {
                        using (var pem = new PemReader(ficheroClave))
                        {
                            claveRSA = pem.ReadRsaKey();
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error($"ERR SERVICIOSEGURIDAD (GetProveedorRSA) - {ex.Message}");
                }
            }

            rsa.ImportParameters(claveRSA);
            return rsa;
        }

        public static void GenerarClavesRSA(string ficheroClavePublica, string ficheroClavePrivada, int keySize = 2048) 
        {           
            var rsa = RSA.Create();
            rsa.KeySize = keySize;
            
            try
            {
                using (var fs = File.Create(ficheroClavePublica))
                {
                    using (var pem = new PemWriter(fs))
                    {
                        pem.WritePublicKey(rsa);
                    }
                }

                using (var fs = File.Create(ficheroClavePrivada))
                {
                    using (var pem = new PemWriter(fs))
                    {
                        pem.WritePrivateKey(rsa);
                    }
                }
            }
            catch(Exception ex)
            {
                //log.Error($"ERR SERVICIOSEGURIDAD (GenerarClaves) - {ex.Message}");
                Console.WriteLine($"ERR SERVICIOSEGURIDAD (GenerarClaves) - {ex.Message}");
            }
        }

        // > ----------- AES
        private static Aes GenerarClaveAES()
        {
            Aes clave = Aes.Create();

            return clave;
        }

        private static byte[] CifrarAES(string textoPlano, byte[] clave, byte[] IV)
        {
            byte[] cifrado;

            if(textoPlano == null || textoPlano.Length <= 0)
            {
                throw new ArgumentNullException("textoPlano");
            }
            if(clave == null || clave.Length <= 0)
            {
                throw new ArgumentNullException("clave");
            }
            if (IV == null || IV.Length <= 0)
            {
                throw new ArgumentNullException("IV");
            }

            //Creamos un objeto de tipo AES con una clave y vector de inicializacion (IV) especificos
            using(var aes = Aes.Create())
            {
                aes.Key = clave;
                aes.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform cifrador = aes.CreateEncryptor(aes.Key, aes.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, cifrador, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(textoPlano);
                        }
                        cifrado = msEncrypt.ToArray();
                    }
                }
            }

            return cifrado;
        }

        private static string DescifrarAES(byte[] textoCifrado, byte[] clave, byte[] IV)
        {
            if(textoCifrado == null || textoCifrado.Length <= 0)
                throw new ArgumentNullException("textoCifrado");
            if(clave == null || clave.Length <= 0)
                throw new ArgumentNullException("clave");
            if(IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            string textoPlano = string.Empty;

            using(var aes = Aes.Create())
            {
                aes.Key = clave;
                aes.IV = IV;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream msDecrypt = new MemoryStream(textoCifrado))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            textoPlano = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return textoPlano;
        }

        public EntidadPeticion ToEntidadPeticion(EntidadPeticionSegura peticionSegura)
        {
            EntidadPeticion entidadPeticion;

            try
            {
                byte[] clave = DescifrarRSA(peticionSegura.Clave);
                byte[] IV = DescifrarRSA(peticionSegura.IV);

                string peticion = DescifrarAES(peticionSegura.Peticion, clave, IV);
                
                entidadPeticion = JsonConvert.DeserializeObject<EntidadPeticion>(peticion);
            }
            catch(Exception ex)
            {
                entidadPeticion = null;
                //log.Error($"SERVICIOSEGURIDAD (ToEntidadPeticion) - {ex.Message}");
                Console.WriteLine($"SERVICIOSEGURIDAD (ToEntidadPeticion) - {ex.Message}");                
            }

            return entidadPeticion;
        }

        public EntidadPeticionSegura ToEntidadPeticionSegura(EntidadPeticion peticion)
        {
            EntidadPeticionSegura entidadPeticionSegura;
            string peticionSerializada = JsonConvert.SerializeObject(peticion);

            try
            {
                Aes aes = GenerarClaveAES();
                byte[] peticionSegura = CifrarAES(peticionSerializada, aes.Key, aes.IV);

                entidadPeticionSegura = new EntidadPeticionSegura()
                {
                    Clave = CifrarRSA(aes.Key),
                    IV = CifrarRSA(aes.IV),
                    Peticion = peticionSegura 
                };
            }
            catch(Exception ex)
            {
                entidadPeticionSegura = null;
                //log.Error($"SERVICIOSEGURIDAD (ToEntidadPeticionSegura) - {ex.Message}");
                Console.WriteLine($"SERVICIOSEGURIDAD (ToEntidadPeticionSegura) - {ex.Message}");
            }

            return entidadPeticionSegura;
        }
    }
}
