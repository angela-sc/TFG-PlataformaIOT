using Libreria.Entidades;
using Libreria.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repositorio.SQLServer
{
    public class RepositorioSensor : IRepositorioSensor
    {
        private string conexionBD;

        public RepositorioSensor(string conexionBD)
        {
            this.conexionBD = conexionBD;
        }

        public void InsertaDato(EntidadDato dato)
        {
            throw new NotImplementedException();
        }

        public void InsertaSensor(EntidadSensor sensor)
        {

        }
    }
}
