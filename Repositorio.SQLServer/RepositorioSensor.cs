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
            //query sql para insertar los datos en la tabla
            string query;

            using(SqlConnection con = new SqlConnection(conexionBD)){

            }

            throw new NotImplementedException();
        }

        public void InsertaSensor(EntidadSensor sensor)
        {
            throw new NotImplementedException();
        }

        public void InsertaUsuario(EntidadUsuario usuario)
        {
            throw new NotImplementedException();
        }
    }
}
