using Libreria.Entidades;
using Libreria.Interfaces;
using Microsoft.Extensions.Configuration;
using Repositorio.SQLServer;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;


namespace Servicios
{
    public class ServicioInsertaInformacion : IServicioInsertaInformacion
    {

        private ILogger log;

        private IRepositorioSensor repositorioSensor;
        private IRepositorioEstacionBase repositorioEstacion;

        public ServicioInsertaInformacion(ILogger logger, string connectionString)
        {
            this.log = logger;
            this.repositorioSensor = new RepositorioSensor(connectionString, log);
            this.repositorioEstacion = new RepositorioEstacionBase(connectionString, log);
        }

        public async Task<bool> InsertaPeticion(EntidadPeticion entidadPeticion)
        {           
            string nombreEstacionBase = entidadPeticion.EstacionBase;
            Console.WriteLine(nombreEstacionBase);
            string nombreSensor = entidadPeticion.Sensor;

            //obtenemos los datos: id de la estacion e id del sensor
            int estacionID = await repositorioEstacion.GetId(nombreEstacionBase);
            int sensorID = await repositorioSensor.GetId(nombreSensor, estacionID);

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
                    //log.Debug("Fallo en ServicioInsertaInformacion en el método InsertaPeticion");
                    log.Error($"Error al insertar los datos: No existe la estacion '{nombreEstacionBase}' en la base de datos.");
                }
                
                if(sensorID == -1)
                {
                    //Console.WriteLine($"No existe el sensor '{nombreSensor}' en la base de datos.");
                    //log.Debug("Fallo en ServicioInsertaInformacion en el método InsertaPeticion");
                    log.Error($"Error al insertar los datos: No existe el sensor '{nombreSensor}' en la base de datos.");
                }
                return false;
            }
        }
    }
}
