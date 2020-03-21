using Dapper;
using Libreria.Entidades;
using Libreria.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio.SQLServer
{
    public class RepositorioSensor : IRepositorioSensor
    {
        private string conexionBD;

        public RepositorioSensor(string conexionBD)
        {
            this.conexionBD = conexionBD;
        }

        public async Task InsertaDato(EntidadDato dato)
        {
            Dictionary<string, object> queryParams = new Dictionary<string, object>
            {
                { "@stamp", dato.stamp },
                { "@fk_sensor", dato.FK_sensorID },
                { "@humity", dato.humity },
                { "@temperature", dato.temperature }
            };

            //query sql para insertar los datos en la tabla
            string query = @"INSERT INTO Data([Stamp],[FK_SensorId],[humity],[temperature]) 
                             VALUES (@stamp,@fk_sensor,@humity,@temperature)";
            
            //nueva conexion con la BD. Al utilizar using nos aseguramos de que se libera la sesion
            using(SqlConnection con = new SqlConnection(conexionBD)){
                con.Open(); //abrimos la conexion con la BD

                //comando sql
                SqlCommand sqlCommand = new SqlCommand(query, con);
                sqlCommand.ExecuteNonQuery();

                await con.ExecuteAsync(query, queryParams);
            }

            throw new NotImplementedException();
        }

        public void InsertaSensor(EntidadSensor sensor)
        {
            throw new NotImplementedException();
        }
    }
}
