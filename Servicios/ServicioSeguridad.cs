﻿
using Libreria.Entidades;
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
    public class ServicioSeguridad
    {
        private ILogger log;
        private string fichero;

        public ServicioSeguridad(string fichero, ILogger log)
        {
            this.fichero = fichero;
            this.log = log;
        }

        public string CifrarRSA(string textoPlano)
        {
            string textoCifrado = string.Empty;
            
            try
            {
                using (var rsa = GetProveedorRSA())
                {
                    var textoPlanoBytes = Encoding.Unicode.GetBytes(textoPlano);
                    var textoCifradoBytes = rsa.Encrypt(textoPlanoBytes, RSAEncryptionPadding.Pkcs1);
                    textoCifrado = Convert.ToBase64String(textoCifradoBytes);
                }
            }
            catch(Exception ex)
            {
                //log.Error($"ERR SERVICIOSEGURIDAD (CifrarRSA) - {ex.Message}");
                Console.WriteLine($"ERR SERVICIOSEGURIDAD (CifrarRSA) - {ex.Message}");
            }

            return textoCifrado;
        }

        public string DescifrarRSA(string textoCifrado)
        {
            var textoPlano = string.Empty;

            try
            {
                using (var rsa = GetProveedorRSA())
                {
                    var cipherTextBytes = Convert.FromBase64String(textoCifrado);
                    var textoPlanoBytes = rsa.Decrypt(cipherTextBytes, RSAEncryptionPadding.Pkcs1);
                    textoPlano = Encoding.Unicode.GetString(textoPlanoBytes);

                }
            }
            catch(Exception ex)
            {
                log.Error($"ERR SERVICIOSEGURIDAD (DescifrarRSA) - {ex.Message}");
            }

            return textoPlano;
        }
        
        private RSA GetProveedorRSA()
        {
            var rsa = RSA.Create();
            
            try
            {
                using (var ficheroClave = File.OpenRead(fichero))
                {
                    using (var pem = new PemReader(ficheroClave))
                    {
                        var rsaParameters = pem.ReadRsaKey();
                        rsa.ImportParameters(rsaParameters);
                    }
                }    
            }
            catch(Exception ex)
            {
                log.Error($"ERR SERVICIOSEGURIDAD (GetProveedorRSA) - {ex.Message}");
            }

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
        public static Aes GenerarClaveAES()
        {
            Aes clave = Aes.Create();

            return clave;
        }

        public static byte[] CifrarAES(string textoPlano, byte[] clave, byte[] IV)
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

        public static string DescifrarAES(byte[] textoCifrado, byte[] clave, byte[] IV)
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
                string clave = DescifrarRSA(peticionSegura.Clave);
                string IV = DescifrarRSA(peticionSegura.IV);

                string peticion = DescifrarAES(peticionSegura.Peticion, Convert.FromBase64String(clave), Convert.FromBase64String(IV));
                
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

        public EntidadPeticionSegura ToEntidadPeticionSegura(string peticion)
        {
            EntidadPeticionSegura entidadPeticionSegura;

            try
            {
                Aes aes = GenerarClaveAES();
                byte[] peticionSegura = CifrarAES(peticion, aes.Key, aes.IV);

                entidadPeticionSegura = new EntidadPeticionSegura()
                {
                    Clave = CifrarRSA(Convert.ToBase64String(aes.Key)),
                    IV = CifrarRSA(Convert.ToBase64String(aes.IV)),
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
