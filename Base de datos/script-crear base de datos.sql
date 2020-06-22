USE plataforma_iot
GO

DECLARE @DatosDePrueba BIT = 1 --> 1 = Sí, 0 = No

--ELIMINAR TABLAS DE LA BASE DE DATOS
DROP TABLE IF EXISTS [plataforma_iot]..[Datos];
DROP TABLE IF EXISTS [plataforma_iot]..[Sensor];
DROP TABLE IF EXISTS [plataforma_iot]..[EstacionBase];
DROP TABLE IF EXISTS [plataforma_iot]..[Usuario_en_Proyecto];
DROP TABLE IF EXISTS [plataforma_iot]..[Proyecto];
DROP TABLE IF EXISTS [plataforma_iot]..[Usuario];

DROP TABLE IF EXISTS [dbo].[AspNetRoleClaims]
DROP TABLE IF EXISTS [dbo].[AspNetUserClaims]
DROP TABLE IF EXISTS [dbo].[AspNetUserLogins]
DROP TABLE IF EXISTS [dbo].[AspNetUserRoles]
DROP TABLE IF EXISTS [dbo].[AspNetUserTokens]
DROP TABLE IF EXISTS [dbo].[AspNetRoles]
DROP TABLE IF EXISTS [dbo].[AspNetUsers]
DROP TABLE IF EXISTS [dbo].[__EFMigrationsHistory]

--CREAR TABLAS DE LA BASE DE DATOS
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](256) NULL,
	[NormalizedName] [nvarchar](256) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](450) NOT NULL,
	[UserName] [nvarchar](256) NULL,
	[NormalizedUserName] [nvarchar](256) NULL,
	[Email] [nvarchar](256) NULL,
	[NormalizedEmail] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEnd] [datetimeoffset](7) NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
 CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

CREATE TABLE [dbo].[AspNetRoleClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

ALTER TABLE [dbo].[AspNetRoleClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE

ALTER TABLE [dbo].[AspNetRoleClaims] CHECK CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId]

CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE

ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId]

CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](128) NOT NULL,
	[ProviderKey] [nvarchar](128) NOT NULL,
	[ProviderDisplayName] [nvarchar](max) NULL,
	[UserId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE

ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId]

CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](450) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE

ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId]

ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE

ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId]

CREATE TABLE [dbo].[AspNetUserTokens](
	[UserId] [nvarchar](450) NOT NULL,
	[LoginProvider] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](128) NOT NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[LoginProvider] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

ALTER TABLE [dbo].[AspNetUserTokens]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE

ALTER TABLE [dbo].[AspNetUserTokens] CHECK CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId]

--CREATE TABLE [plataforma_iot]..[Usuario](
--	[id]			INT IDENTITY(1,1)	PRIMARY KEY,
--	[email]			VARCHAR(48)			UNIQUE	NOT NULL,
--	[contrasenya]	VARBINARY(128)				NOT NULL,
--	[nombre]		VARCHAR(30)					NOT NULL,
--	[apellidos]		VARCHAR(30)					NOT NULL
--);

CREATE TABLE [plataforma_iot]..[Proyecto](
	[id]			INT	IDENTITY(1,1)	PRIMARY KEY,
	[nombre]		VARCHAR(128)		UNIQUE		NOT NULL,
	[descripcion]	NVARCHAR(MAX)
);

CREATE TABLE [plataforma_iot]..[Usuario_en_Proyecto](
	[id_usuario]	NVARCHAR(450),
	[id_proyecto]	INT,
	CONSTRAINT [pk_usuarioenproyecto]			PRIMARY KEY ([id_usuario],[id_proyecto]),
	CONSTRAINT [fk_usuarioenproyecto_usuario]	FOREIGN KEY ([id_usuario])	REFERENCES [plataforma_iot]..[AspNetUsers]([Id])	ON DELETE CASCADE,
	CONSTRAINT [fk_usuarioenproyecto_proyecto]	FOREIGN KEY ([id_proyecto]) REFERENCES [plataforma_iot]..[Proyecto]([id])	ON DELETE CASCADE
);

CREATE TABLE [plataforma_iot]..[EstacionBase](
	[id]				INT IDENTITY(1,1)	PRIMARY KEY,
	[nombre]			VARCHAR(10)			NOT NULL,
	[fk_idproyecto]		INT					NOT NULL,
	CONSTRAINT [uk_estacionbase]			UNIQUE ([nombre],[fk_idproyecto]),
	CONSTRAINT [fk_estacionbase_proyecto]	FOREIGN KEY ([fk_idproyecto]) REFERENCES [plataforma_iot]..[Proyecto]([id]) ON DELETE CASCADE
);

CREATE TABLE [plataforma_iot]..[Sensor](
	[id]				INT IDENTITY(1,1)	PRIMARY KEY,
	[nombre]			CHAR(11)			NOT NULL,
	[longitud]			VARCHAR(15)			NOT NULL,
	[latitud]			VARCHAR(15)			NOT NULL,
	[fk_idestacionbase] INT					NOT NULL,
	CONSTRAINT [uk_sensor]				UNIQUE ([nombre],[fk_idestacionbase]),
	CONSTRAINT [fk_sensor_estacionbase] FOREIGN KEY ([fk_idestacionbase]) REFERENCES [plataforma_iot]..[EstacionBase]([id]) ON DELETE CASCADE
);

CREATE TABLE [plataforma_iot]..[Datos](
	[stamp]			DATETIME,
	[fk_idsensor]	INT,
	[humedad]		FLOAT,
	[temperatura]	FLOAT,
	CONSTRAINT [pk_datos]			PRIMARY KEY ([stamp],[fk_idsensor]),
	CONSTRAINT [fk_datos_sensor]	FOREIGN KEY ([fk_idsensor]) REFERENCES [plataforma_iot]..[Sensor]([id]) ON DELETE CASCADE
);


-- INSERTAR DATOS EN LA BASE DE DATOS
INSERT INTO [dbo].[__EFMigrationsHistory] ([MigrationId],[ProductVersion])
VALUES ('00000000000000_CreateIdentitySchema','3.1.4')

-- DATOS PARA PRUEBAS
IF(@DatosDePrueba = 1)
BEGIN
	---- usuarios
	--prueba@mail.com
	--Pa$$w0rd
	INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) 
	VALUES (N'016ba22d-3dbd-4af6-a6d1-578c9b4af427', N'prueba@mail.com', N'PRUEBA@MAIL.COM', N'prueba@mail.com', N'PRUEBA@MAIL.COM', 0, N'AQAAAAEAACcQAAAAECmTKX3x+H5d3Js1kHb6Bo2Kfglm5/PWsOd0w1Kce7eimN/MoToBI10/gEi0X8bbSg==', N'RWVAUEBDGWDYZ6ZXVHIIXQORW3M4NJ2S', N'c326f6db-e2da-401c-a928-4ef80193a1b4', NULL, 0, 0, NULL, 1, 0)

	--SET IDENTITY_INSERT [plataforma_iot]..[Usuario] ON;

	--INSERT INTO [plataforma_iot]..[Usuario]
	--		   ([id], [email], [contrasenya], [nombre], [apellidos])
	--	 VALUES (0,'kensington@mail.com',CAST('contrasenya' AS VARBINARY(128)),'ken','sington'),
	--			(1, 'roccat@mail.com', CAST('contrasenya' AS VARBINARY(128)),'roccat','kanga');

	--SET IDENTITY_INSERT [plataforma_iot]..[Usuario] OFF;

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
		 VALUES	(N'016ba22d-3dbd-4af6-a6d1-578c9b4af427',1),
				(N'016ba22d-3dbd-4af6-a6d1-578c9b4af427',0),
				(N'016ba22d-3dbd-4af6-a6d1-578c9b4af427',2),
				(N'016ba22d-3dbd-4af6-a6d1-578c9b4af427',3);



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
END

EXEC sp_MSforeachtable 'SELECT * FROM ?';