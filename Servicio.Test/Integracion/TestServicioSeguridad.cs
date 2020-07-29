using Libreria.Entidades;
using Libreria.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Servicios;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;
using System.Text;

namespace Servicio.Test.Integracion
{
    [TestClass]
    public class TestServicioSeguridad
    {
       

        //[TestMethod]
        public void TestCambioEntidades()
        {
            string ficheroClavePublica = @"C:\Users\abox\Development\TFG-Angela\Claves\publica.key";
            string ficheroClavePrivada = @"C:\Users\abox\Development\TFG-Angela\Claves\privada.key";

            ServicioSeguridad servicioAPI = new ServicioSeguridad(ficheroClavePrivada, null);
            ServicioSeguridad servicioWorker = new ServicioSeguridad(ficheroClavePublica, null);

            // > ENTIDAD PETICION INCIAL - SIN SEGURIDAD
            EntidadPeticion inicial = new EntidadPeticion()
            {
                Proyecto = "Matrix",
                EstacionBase = "EB04",
                Sensor = "SE00"
            };

            List<EntidadDatoBase> datos = new List<EntidadDatoBase>();
            DateTime stampInicial = DateTime.UtcNow;
            Random rnd = new Random();
            for (int i = 0; i < 20; i++)
            {
                datos.Add(new EntidadDatoBase() { Humedad = rnd.Next(0, 11), Temperatura = rnd.Next(20, 32), Stamp = stampInicial.AddMinutes(i) });
            }

            inicial.Datos = datos;

            // > ENTIDAD PETICION CIFRADA - CON SEGURIDAD
            EntidadPeticionSegura cifrada = servicioWorker.ToEntidadPeticionSegura(inicial);
            ServicioSeguridad.LimpiaClaveRSA();
            // > ENTIDAD PETICION DESCIFRADA - DESCIFRADA CON SEGURIDAD
            EntidadPeticion descifrada = servicioAPI.ToEntidadPeticion(cifrada);

            // > ASSERTS
            Assert.AreEqual(inicial.Proyecto, descifrada.Proyecto);
            Assert.AreEqual(inicial.EstacionBase, descifrada.EstacionBase);
            Assert.AreEqual(inicial.Sensor, descifrada.Sensor);
            Assert.AreEqual(inicial.Datos.Count(), descifrada.Datos.Count());

            var datosIniciales = inicial.Datos.OrderBy(_ => _.Stamp);
            var datosDescifrados = descifrada.Datos.OrderBy(_ => _.Stamp);
            for(int i = 0; i < datosIniciales.Count(); i++)
            {
                Assert.AreEqual(datosIniciales.ElementAt(i).Stamp.Ticks, datosDescifrados.ElementAt(i).Stamp.Ticks);
                Assert.AreEqual(datosIniciales.ElementAt(i).Humedad, datosDescifrados.ElementAt(i).Humedad);
                Assert.AreEqual(datosIniciales.ElementAt(i).Temperatura, datosDescifrados.ElementAt(i).Temperatura);
            }
        }

        //[TestMethod]
        //public void TestGenerarClavesRSA()
        //{
        //    //ServicioSeguridad.GenerarClavesRSA(@"E:\Descargas\claves\publica.key", @"E:\Descargas\claves\privada.key");
        //}


        //[TestMethod]
        //public void TestCifrarDescifrarRSA()
        //{
        //    string ficheroClavePublica = "C:\\tfg\\clave_publica.key";
        //    string ficheroClavePrivada = "C:\\tfg\\clave_privada.key";

        //    ServicioSeguridad servicioAPI = new ServicioSeguridad(ficheroClavePrivada, null);
        //    ServicioSeguridad servicioWorker = new ServicioSeguridad(ficheroClavePublica, null);

        //    ServicioSeguridad.GenerarClavesRSA(ficheroClavePublica, ficheroClavePrivada, 4096);

        //    string texto = "prueba RSA";
        //    string cifrado = servicioWorker.CifrarRSA(texto);
        //    string descifrado = servicioAPI.DescifrarRSA(cifrado);

        //    Assert.AreEqual(texto, descifrado);
        //    Assert.AreNotEqual(texto, cifrado);
        //}

        //[TestMethod]
        //public void TestCifrarDescifrarAES()
        //{
        //    string texto = "prueba AES";
        //    var claveSimetrica = ServicioSeguridad.GenerarClaveAES();

        //    var clave = ServicioSeguridad.GenerarClaveAES();

        //    byte[] cifrado = ServicioSeguridad.CifrarAES(texto, claveSimetrica.Key, claveSimetrica.IV);
        //    string descifrado = ServicioSeguridad.DescifrarAES(cifrado, claveSimetrica.Key, claveSimetrica.IV);

        //    Assert.AreEqual(texto, descifrado);
        //    Assert.AreNotEqual(texto, cifrado);
        //}

        //[TestMethod]
        //public void TestCifraDescifrarPeticion() {

        //    // > DATOS
        //    EntidadPeticion peticion = new EntidadPeticion()
        //    {
        //        Proyecto = "Matrix",
        //        EstacionBase = "EB04",
        //        Sensor = "SE00"
        //    };

        //    List<EntidadDatoBase> datos = new List<EntidadDatoBase>();
        //    DateTime stampInicial = DateTime.UtcNow;
        //    Random rnd = new Random();
        //    for (int i = 0; i < 11; i++)
        //    {
        //        datos.Add(new EntidadDatoBase() { Humedad = rnd.Next(0,11) , Temperatura = rnd.Next(20, 32), Stamp = stampInicial.AddMinutes(i)});
        //    }

        //    peticion.Datos = datos;

        //    // > CIFRADO

        //    string ficheroClavePublica = "C:\\tfg\\claves\\clave_publica.key";
        //    string ficheroClavePrivada = "C:\\tfg\\claves\\clave_privada.key";

        //    ServicioSeguridad servicioAPI = new ServicioSeguridad(ficheroClavePrivada, null);
        //    ServicioSeguridad servicioWorker = new ServicioSeguridad(ficheroClavePublica, null);

        //    ServicioSeguridad.GenerarClavesRSA(ficheroClavePublica, ficheroClavePrivada, 4096);

        //    EntidadPeticionSegura entidadPeticionSegura = servicioWorker.ToEntidadPeticionSegura(JsonConvert.SerializeObject(peticion));
        //    EntidadPeticion entidadPeticion = servicioAPI.ToEntidadPeticion(entidadPeticionSegura);

        //    // > ASSERTS
        //    Assert.AreEqual(peticion.Proyecto, entidadPeticion.Proyecto);
        //}

        //[TestMethod]
        //public void TestCifrarAESConRSA()
        //{
        //    var aes = ServicioSeguridad.GenerarClaveAES();
        //    string ficheroClavePublica = "C:\\tfg\\claves\\clave_publica.key";
        //    string ficheroClavePrivada = "C:\\tfg\\claves\\clave_privada.key";

        //    ServicioSeguridad servicioAPI = new ServicioSeguridad(ficheroClavePrivada, null);
        //    ServicioSeguridad servicioWorker = new ServicioSeguridad(ficheroClavePublica, null);

        //    string cifrado = servicioWorker.CifrarRSA(Convert.ToBase64String(aes.Key));
        //    string descifrado = servicioAPI.DescifrarRSA(cifrado);

        //    byte[] final = Convert.FromBase64String(descifrado);

        //    Assert.AreEqual(Convert.ToBase64String(aes.Key), descifrado);
        //}
    }
}
