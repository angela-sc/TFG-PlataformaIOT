using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio.Test.Herramientas
{
    public class BaseDatosTest
    {
        private readonly string conexion;

        public BaseDatosTest(string cadenaConexion)
        {
            conexion = cadenaConexion;
        }

		public async Task BorraDatos()
		{
			using (SqlConnection conn = new SqlConnection(conexion))
			{
				await conn.ExecuteAsync(BorrarDatos);
			}
		}

		public async Task InsertaDatosTest()
		{
			using (SqlConnection conn = new SqlConnection(conexion))
			{
				await conn.ExecuteAsync(DatosTest);
			}
		}

        public async Task Reinicia()
        {
            using (SqlConnection conn = new SqlConnection(conexion))
            {
                await conn.ExecuteAsync(BorrarDatos);
				await conn.ExecuteAsync(DatosTest);
            }
        }

		private string BorrarDatos = @"
		DELETE FROM [plataforma_iot].[dbo].[Proyecto];
		DELETE FROM [plataforma_iot].[dbo].[Usuario];
		";

        private string DatosTest = @"
		---- usuarios
		SET IDENTITY_INSERT [plataforma_iot]..[Usuario] ON;

		INSERT INTO [plataforma_iot]..[Usuario]
				   ([id], [email], [contrasenya], [nombre], [apellidos])
			 VALUES (0,'kensington@mail.com',CAST('contrasenya' AS VARBINARY(128)),'ken','sington'),
					(1, 'roccat@mail.com', CAST('contrasenya' AS VARBINARY(128)),'roccat','kanga');

		SET IDENTITY_INSERT [plataforma_iot]..[Usuario] OFF;

		---- proyectos
		SET IDENTITY_INSERT [plataforma_iot]..[Proyecto] ON;

		INSERT INTO [plataforma_iot]..[Proyecto]
					([id],[nombre],[descripcion])
			VALUES	(0,'Escarlet', 'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.'),
					(1,'Neo','Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.'),
					(2,'Escorpio','Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.'),
					(3,'Lockheart','Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.');

		SET IDENTITY_INSERT [plataforma_iot]..[Proyecto] OFF;

		---- usuario en proyecto


		INSERT INTO [plataforma_iot]..[Usuario_en_Proyecto]
				   ([id_usuario],[id_proyecto])
			 VALUES	(0,1),
					(1,0),
					(1,2),
					(1,3);



		---- estaciones base
		SET IDENTITY_INSERT [plataforma_iot]..[EstacionBase] ON;

		INSERT INTO [plataforma_iot]..[EstacionBase]
					([id],[nombre],[fk_idproyecto])
				VALUES	(0,'EB00',0),
						(1,'EB01',0),
						(2,'EB02',1),
						(3,'EB03',2);

		SET IDENTITY_INSERT [plataforma_iot]..[EstacionBase] OFF;

		---- sensores
		SET IDENTITY_INSERT [plataforma_iot]..[Sensor] ON;

		INSERT INTO [plataforma_iot]..[Sensor]
				   ([id],[nombre],[longitud],[latitud],[fk_idestacionbase])
			 VALUES
				   (0,'SE00','-0,780410','38,193270',0),
				   (1,'SE01','-0,782856','38,190015',0),
				   (6,'SE02','-0,743588','38,169475',0),
				   (7,'SE03','-0,751807','38,166119',0),
				   (8,'SE04','-0,717310','38,173168',0),
				   (2,'SE10','-0,528031','38,2781293',1),
				   (3,'SE11','-0,530436','38,280800',1),
				   (4,'SE12','-0,530436','38,280800',2),
				   (5,'SE03','-0,535455','38,275500',3);

		SET IDENTITY_INSERT [plataforma_iot]..[Sensor] OFF;

		---- datos

		INSERT INTO [plataforma_iot]..[Datos]
				   ([stamp],[fk_idsensor],[humedad],[temperatura])
			 VALUES
				   ('2020-03-31 11:55:00',0,40.75,20.05),
				   ('2020-03-31 12:00:00',0,41.75,20.05),
				   ('2020-03-31 12:05:00',0,40.01,20.05),
				   ('2020-03-31 12:10:00',0,40.5,20.05),

				   ('2020-03-31 11:55:00',1,35.7,25.53),
				   ('2020-03-31 12:00:00',1,35.8,25.55),
				   ('2020-03-31 12:05:00',1,35.95,25.53),

				   ('2020-03-31 11:55:00',2,35.7,25.53),
				   ('2020-03-31 12:00:00',2,35.8,25.55),
				   ('2020-03-31 12:05:00',2,35.95,25.54),
				   ('2020-03-31 12:10:00',2,35.89,25.55),
				   ('2020-03-31 12:15:00',2,35.75,25.56),
				   ('2020-03-31 12:20:00',2,35.8,25.53),

				   ('2020-03-31 11:55:00',3,35.7,25.53),
				   ('2020-03-31 12:00:00',3,35.8,25.55),
				   ('2020-03-31 12:05:00',3,35.95,25.54),

				   ('2020-03-31 11:55:00',4,35.7,25.53),
				   ('2020-03-31 12:00:00',4,30.8,20.55),
				   ('2020-03-31 12:05:00',4,32.95,29.54),

				   ('2020-03-31 11:55:00',5,35.7,25.53),
				   ('2020-03-31 12:00:00',5,35.8,25.55),
				   ('2020-03-31 12:05:00',5,35.95,25.54),
				   ('2020-03-31 12:10:00',5,35.89,25.55),
				   ('2020-03-31 12:15:00',5,35.75,25.56),

				   ('2020-03-31 11:50:00',6,35.8,26.0245),
				   ('2020-03-31 11:55:00',6,35.7,25.53),
				   ('2020-03-31 12:00:00',6,35.8,24.55),
				   ('2020-03-31 12:05:00',6,35.95,25.54),
				   ('2020-03-31 12:10:00',6,32.89,28.55),
				   ('2020-03-31 12:15:00',6,33.75,29.56),
				   ('2020-03-31 12:20:00',6,37.8,30.53),

				   ('2020-03-31 11:55:00',7,35.7,20.753),
				   ('2020-03-31 12:00:00',7,34.8,24.579),
				   ('2020-03-31 12:05:00',7,35.95,22.169),

				   ('2020-03-31 12:10:00',8,35.89,20.55),
				   ('2020-03-31 12:15:00',8,34.983,21.56),
				   ('2020-03-31 12:20:00',8,35.218,22.53);
        ";
    }
}
