using Libreria.Entidades;
using Libreria.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
            string query = @"INSERT INTO Data([Stamp],[FK_SensorId],[humity],[temperature]) 
                             VALUES (@stamp,@fk_sensor,@humity,@temperature)";
            
            //nueva conexion con la BD. Al utilizar using nos aseguramos de que se libera la sesion
            using(SqlConnection con = new SqlConnection(conexionBD)){
                con.Open(); //abrimos la conexion con la BD

                //comando sql
                SqlCommand sqlCommand = new SqlCommand(query, con);
                sqlCommand.ExecuteNonQuery();
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
