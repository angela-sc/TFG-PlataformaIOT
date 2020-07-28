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

        public ServicioInsertaInformacion(string cadenaConexion, ILogger logger)
        {
            this.log = logger;
            this.repositorioSensor = new RepositorioSensor(cadenaConexion, log);
            this.repositorioEstacion = new RepositorioEstacionBase(cadenaConexion, log);
        }

        public async Task<bool> InsertaPeticion(EntidadPeticion entidadPeticion)
        {
            if (entidadPeticion == null)
                return false;

            string nombreProyecto = entidadPeticion.Proyecto;
            string nombreEstacionBase = entidadPeticion.EstacionBase;
            string nombreSensor = entidadPeticion.Sensor;

            int estacionId = -1, sensorId = -1;
            bool result = true; //booleano para saber si ha ocurrido un error durante la insercion de datos

            try
            {
                //obtenemos los datos: id de la estacion e id del sensor
                estacionId = await repositorioEstacion.ObtenerId(nombreProyecto, nombreEstacionBase);
                sensorId = await repositorioSensor.ObtenerId(nombreSensor, estacionId);

                if (estacionId != -1 && sensorId != -1)
                {
                    foreach (EntidadDatoBase datoBase in entidadPeticion.Datos)
                    {
                        var dato = new EntidadDato();

                        dato.Stamp = datoBase.Stamp;
                        dato.Humedad = datoBase.Humedad;
                        dato.Temperatura = datoBase.Temperatura;
                        dato.FK_IdSensor = sensorId;

                        result = (result && await repositorioSensor.InsertaDato(dato)); //si falla alguna da false
                    }
                    //leer entidadPeticion e insertar en las tablas correspondientes usando los repositorio
                }
                else
                {
                    if (estacionId == -1)
                    {
                        //log.Debug("Fallo en ServicioInsertaInformacion en el método InsertaPeticion");
                        log.Warning($"ERR. SERVICIO INSERTA INFORMACION (InsertaPeticion) -  No existe la estacion '{nombreEstacionBase}' en la base de datos");
                    }

                    if (sensorId == -1)
                    {
                        //log.Debug("Fallo en ServicioInsertaInformacion en el método InsertaPeticion");
                        log.Warning($"ERR. SERVICIO INSERTA INFORMACION (InsertaPeticion) - No existe el sensor {sensorId} en la base de datos");
                    }
                    result = false;
                }
            }
            catch(Exception ex)
            {
                log.Error($"ERR. SERVICIO INSERTA INFORMACION (InsertaPeticion) -{ex.Message}");
                result = false;
            }

            return result;
        }
    }
}
