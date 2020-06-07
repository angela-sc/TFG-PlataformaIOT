using Libreria.Entidades;
using Libreria.Interfaces;
using Microsoft.Extensions.DependencyModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repositorio.SQLServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Resources;
using Repositorio.Test.Herramientas;

namespace Repositorio.Test.Integracion
{
    [TestClass]
    public class TestRepositorioSensor
    {
        private static IRepositorioSensor repositorioSensor;
        private static string cadenaConexion = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=plataforma_iot;Integrated Security=true";
        private static BaseDatosTest baseDatosTest;

        private static int idSensor = 0;

        //[TestInitialize] //se ejecuta antes de cada testmethod
        public void TestInitialize() 
        {
        }

        [TestCleanup] //se ejecuta despues de cada testmethod
        public async Task TestCleanup() => await baseDatosTest.Reinicia();

        [ClassInitialize()] //se ejecuta antes de TODOS los tests
        public static async Task InitTestSuite(TestContext testContext) 
        {
            repositorioSensor = new RepositorioSensor(cadenaConexion, null);
            baseDatosTest = new BaseDatosTest(cadenaConexion);

            await baseDatosTest.Reinicia();
        }

        //[ClassCleanup()] //se ejecuta despues de TODOS los tests
        public static void CleanupTestSuite() 
        {
            
        }

        [TestMethod]
        public async Task TestInsertaDato()
        {
            DateTime fechaDatoInsertado = DateTime.UtcNow;
            float humedad = 23, temperatura = 25;

            EntidadDato dato = new EntidadDato()
            {
                Humedad = humedad,
                Temperatura = temperatura,
                FK_IdSensor = idSensor,
                Stamp = fechaDatoInsertado
            };

            try
            {
                var resultado = await repositorioSensor.InsertaDato(dato);
                Assert.IsTrue(resultado);
            }
            catch(Exception ex)
            {
                Assert.Fail($"Error InsertaDato: {ex.Message}");
            }

            try
            {
                var datosBd = await repositorioSensor.ObtenerDatos(idSensor, fechaDatoInsertado.AddSeconds(-1), fechaDatoInsertado.AddSeconds(1));
                Assert.IsNotNull(datosBd);
                Assert.AreEqual(1, datosBd.Count());
                Assert.AreEqual(humedad, datosBd.FirstOrDefault().Humedad);
                Assert.AreEqual(temperatura, datosBd.FirstOrDefault().Temperatura);
                Assert.AreEqual(fechaDatoInsertado.Second, datosBd.FirstOrDefault().Stamp.Second);
            }
            catch(Exception ex)
            {
                Assert.Fail($"Error ObtenerDatos: {ex.Message}");
            }
        }
    }
}
