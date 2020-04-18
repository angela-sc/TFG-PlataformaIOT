using Libreria.Entidades;
using Libreria.Interfaces;
using Repositorio.SQLServer;
using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public class ServicioEstacionBase
    {
        private IRepositorioEstacionBase repositorioEstacionBase;

        private string cadenaConexion;
        private ILogger log;

        public ServicioEstacionBase(string cadenaConexion, ILogger log)
        {
            this.cadenaConexion = cadenaConexion;
            this.log = log;

            this.repositorioEstacionBase = new RepositorioEstacionBase(cadenaConexion, log);
        }

        public async Task<IEnumerable<EntidadSensorResultado>> ObtenerSensores(string nombreEstacionBase)
        {
            IEnumerable<EntidadSensorResultado> sensores = new List<EntidadSensorResultado>();
            sensores = await repositorioEstacionBase.ObtenerSensores(nombreEstacionBase);

            return sensores;
        }

       
    }
}
