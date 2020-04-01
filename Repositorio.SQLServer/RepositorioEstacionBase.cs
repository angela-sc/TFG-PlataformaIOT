﻿using Dapper;
using Libreria.Entidades;
using Libreria.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

using Repositorio.SQLServer;

namespace Repositorio.SQLServer
{
    public class RepositorioEstacionBase : IRepositorioEstacionBase
    {
        private string connectionString;

        public RepositorioEstacionBase(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public int GetId(string nombreEstacionBase)
        {
            string query = String.Format( "SELECT [Id] FROM [plataformadb].[dbo].[Base_station] WHERE [Name]= '{0}'", nombreEstacionBase);
           
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    var res = conn.QueryAsync<int>(query);                    
                    return res.Result.FirstOrDefault();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Método GetId de Repositorio.EstacionBase: "+ ex.Message);
                return -1;
            }        
        }

        public void InsertaEstacion(EntidadEstacionBase entidadEstacion)
        {
            throw new NotImplementedException();
        }

    }
}