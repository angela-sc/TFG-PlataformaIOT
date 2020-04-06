using Libreria.Entidades;
using Libreria.Interfaces;
using Microsoft.Extensions.Configuration;
using Repositorio.SQLServer;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace Servicios
{
    public class ServicioInsertaInformacion : IServicioInsertaInformacion
    {
        private string connectionString;  //cadena de conexion de la base de datos

        private ILogger logger;

        private IRepositorioSensor repositorioSensor;
        private IRepositorioEstacionBase repositorioEstacion;

        public ServicioInsertaInformacion(ILogger logger)
        {
            this.logger = logger;

            var configuration = GetConfiguration();

            //obtenemos la cadena de conexion del fichero de configuracion @$"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=plataformadb;Integrated Security=true";
            this.connectionString = configuration["ConnectionString"];

            this.repositorioSensor = new RepositorioSensor(connectionString);
            this.repositorioEstacion = new RepositorioEstacionBase(connectionString);
        }

        public async Task<bool> InsertaPeticion(EntidadPeticion entidadPeticion)
        {           
            string nombreEstacionBase = entidadPeticion.EstacionBase;
            string nombreSensor = entidadPeticion.Sensor;

            //obtenemos los datos: id de la estacion e id del sensor
            int estacionID = repositorioEstacion.GetId(nombreEstacionBase);
            int sensorID = repositorioSensor.GetId(nombreSensor, estacionID);

            bool result = true; //booleano para saber si ha ocurrido un error durante la insercion de datos

            if(estacionID != -1 && sensorID != -1)
            {
                foreach (EntidadDatoBase datoBase in entidadPeticion.Datos)
                {
                    var dato = new EntidadDato();

                    dato.stamp = datoBase.stamp;
                    dato.humity = datoBase.humity;
                    dato.temperature = datoBase.temperature;
                    dato.FK_sensorID = sensorID;

                    result = (result && await repositorioSensor.InsertaDato(dato)); //si falla alguna da false
                }
                //leer entidadPeticion e insertar en las tablas correspondientes usando los repositorio
                return result;               
            }
            else
            {
                if(estacionID == -1)
                {
                    //Console.WriteLine($"No existe la estacion '{nombreEstacionBase}' en la base de datos.");
                    logger.Error($"Error al insertar los datos: No existe la estacion '{nombreEstacionBase}' en la base de datos.");
                }
                
                if(sensorID == -1)
                {
                    //Console.WriteLine($"No existe el sensor '{nombreSensor}' en la base de datos.");
                    logger.Error($"Error al insertar los datos: No existe el sensor '{nombreSensor}' en la base de datos.");
                }
                return false;
            }
        }

        //Metodo que devuelve un objeto IConfiguration para poder acceder a la informacion del fichero settings.json        
        private IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            return builder.Build();
        }
    }
}
