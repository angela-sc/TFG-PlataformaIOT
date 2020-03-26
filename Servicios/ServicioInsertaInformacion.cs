using Libreria.Entidades;
using Libreria.Interfaces;
using Repositorio.SQLServer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public class ServicioInsertaInformacion : IServicioInsertaInformacion
    {
        private string connectionString; 

        private IRepositorioSensor repositorioSensor;
        private IRepositorioEstacionBase repositorioEstacion;

        public ServicioInsertaInformacion()
        {
            this.connectionString = @$"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=plataformadb;Integrated Security=true";

            this.repositorioSensor = new RepositorioSensor(connectionString);
            this.repositorioEstacion = new RepositorioEstacionBase(connectionString);
        }

        public async Task InsertaPeticion(EntidadPeticion entidadPeticion)
        {
            string nombreEstacionBase = entidadPeticion.EstacionBase;
            string nombreSensor = entidadPeticion.Sensor;

            int estacionID = repositorioEstacion.GetId(nombreEstacionBase);
            int sensorID = repositorioSensor.GetId(nombreSensor, estacionID);


            foreach(EntidadDatoBase datoBase in entidadPeticion.Datos)
            {
                var dato = new EntidadDato();
                dato.stamp = datoBase.stamp;
                dato.humity = datoBase.humity;
                dato.temperature = datoBase.temperature;
                dato.FK_sensorID = sensorID;

               await repositorioSensor.InsertaDato(dato);
            }
            //leer entidadPeticion e insertar en las tablas correspondientes usando los repositorios
        }
    }
}
