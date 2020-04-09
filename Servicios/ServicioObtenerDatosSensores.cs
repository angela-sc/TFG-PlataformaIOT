using Libreria.Entidades;
using Repositorio.SQLServer;
using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public class ServicioObtenerDatosSensores
    {
      
        private ILogger log;
        private RepositorioSensor repositorioSensor; //instancia para acceder a la base de datos

        public ServicioObtenerDatosSensores(string cadenaConexion, ILogger logger)
        {
            this.log = logger;
            this.repositorioSensor = new RepositorioSensor(cadenaConexion, log);
        }

        //Metodo que obtiene los datos existentes de un sensor dado su id
        // ?? se busca por id o por nombre sensor + estacion base ??
        public async Task<List<EntidadDatoBase>> GetTaskAsync(int idSensor)
        {
            List<EntidadDatoBase> datos = null; //inicialmente esta vacia
            
            datos = await repositorioSensor.GetData(idSensor);

            return datos;

        }
    }
}
