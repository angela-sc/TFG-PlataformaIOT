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
        private string conexionBD;

        private IRepositorioSensor repositorioSensor;

        public ServicioInsertaInformacion(string conexionBD)
        {
            this.conexionBD = conexionBD;

            this.repositorioSensor = new RepositorioSensor(conexionBD);
        }

        public async Task InsertaPeticion(EntidadPeticion entidadPeticion)
        {
            EntidadDato dato;
            // TODO: leer entidadPeticion e insertar en las tablas correspondientes usando los repositorios

        }
    }
}
