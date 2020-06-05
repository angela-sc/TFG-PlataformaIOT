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

        public ServicioInsertaInformacion(ILogger logger, string cadenaConexion)
        {
            this.log = logger;
            this.repositorioSensor = new RepositorioSensor(cadenaConexion, log);
            this.repositorioEstacion = new RepositorioEstacionBase(cadenaConexion, log);
        }

        public async Task<bool> InsertaPeticion(EntidadPeticion entidadPeticion)
        {           
            string nombreEstacionBase = entidadPeticion.EstacionBase;
            string nombreSensor = entidadPeticion.Sensor;

            //obtenemos los datos: id de la estacion e id del sensor
            int estacionID = await repositorioEstacion.ObtenerId(nombreEstacionBase);
            int sensorID = await repositorioSensor.ObtenerId(nombreSensor, estacionID);

            bool result = true; //booleano para saber si ha ocurrido un error durante la insercion de datos

            if(estacionID != -1 && sensorID != -1)
            {
                foreach (EntidadDatoBase datoBase in entidadPeticion.Datos)
                {
                    var dato = new EntidadDato();

                    dato.Stamp = datoBase.Stamp;
                    dato.Humedad = datoBase.Humedad;
                    dato.Temperatura = datoBase.Temperatura;
                    dato.FK_IdSensor = sensorID;

                    result = (result && await repositorioSensor.InsertaDato(dato)); //si falla alguna da false
                }
                //leer entidadPeticion e insertar en las tablas correspondientes usando los repositorio
                return result;               
            }
            else
            {
                if(estacionID == -1)
                {                    
                    //log.Debug("Fallo en ServicioInsertaInformacion en el método InsertaPeticion");
                    log.Error($" No existe la estacion '{nombreEstacionBase}' en la base de datos - ERR. SERVICIO INSERTA INFORMACION");
                }
                
                if(sensorID == -1)
                {                 
                    //log.Debug("Fallo en ServicioInsertaInformacion en el método InsertaPeticion");
                    log.Error($"No existe el sensor '{nombreSensor}' en la base de datos - ERR. SERVICIO INSERTA INFORMACION");
                }
                return false;
            }
        }
    }
}
