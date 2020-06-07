using Libreria.Entidades;
using Libreria.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repositorio.SQLServer;
using Repositorio.Test.Herramientas;
using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicio.Test.Integracion
{
    [TestClass]
    public class TestServicioInsertaInformacion
    {
        private static IRepositorioSensor repositorioSensor;
        private static IServicioInsertaInformacion servicioInsertaInformacion;
        private static string cadenaConexion = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=plataforma_iot;Integrated Security=true";
        private static BaseDatosTest baseDatosTest;

        private static string sensor = "SE00", proyecto = "Escarlet", estacionBase = "EB00";
        private static int idSensor = 0;

        [TestCleanup] //se ejecuta despues de cada testmethod
        public async Task TestCleanup() => await baseDatosTest.Reinicia();

        [ClassInitialize()] //se ejecuta antes de TODOS los tests
        public static async Task InitTestSuite(TestContext testContext)
        {
            repositorioSensor = new RepositorioSensor(cadenaConexion, null);
            servicioInsertaInformacion = new ServicioInsertaInformacion(null, cadenaConexion);
            baseDatosTest = new BaseDatosTest(cadenaConexion);

            await baseDatosTest.Reinicia();
        }

        [TestMethod]
        public async Task TestInsertaInformacion()
        {
            DateTime stampInicial = DateTime.UtcNow;
            float humedad = 23, temperatura = 25;

            var listaDatos = new List<EntidadDatoBase>()
            {
                new EntidadDatoBase() { Humedad = humedad, Temperatura = temperatura, Stamp = stampInicial },
                new EntidadDatoBase() { Humedad = humedad, Temperatura = temperatura, Stamp = stampInicial.AddMinutes(1) },
                new EntidadDatoBase() { Humedad = humedad, Temperatura = temperatura, Stamp = stampInicial.AddMinutes(2) },
                new EntidadDatoBase() { Humedad = humedad, Temperatura = temperatura, Stamp = stampInicial.AddMinutes(3) },
                new EntidadDatoBase() { Humedad = humedad, Temperatura = temperatura, Stamp = stampInicial.AddMinutes(4) }
            };

            EntidadPeticion peticion = new EntidadPeticion()
            {
                Sensor = sensor,
                Proyecto = proyecto,
                EstacionBase = estacionBase,
                Datos = listaDatos
            };

            try
            {
                var resultado = await servicioInsertaInformacion.InsertaPeticion(peticion);
                Assert.IsNotNull(resultado);
                Assert.IsTrue(resultado);
            }
            catch(Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            try
            {
                var datosBd = await repositorioSensor.ObtenerDatos(idSensor, stampInicial.AddMinutes(-1), stampInicial.AddMinutes(5));
                Assert.IsNotNull(datosBd);
                Assert.AreEqual(listaDatos.Count(), datosBd.Count());
            }
            catch(Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}
