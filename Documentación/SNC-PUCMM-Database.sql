CREATE TABLE TipoUsuario (
	idTipoUsuario serial,
	nombre text,
	descripcion text,
	CONSTRAINT PK_TipoUsuario PRIMARY KEY (idTipoUsuario)
);

CREATE TABLE CuentaFacebook (
	idCuentaFacebook serial,
	usuarioFacebook text,
	token text,
	CONSTRAINT PK_CuentaFacebook PRIMARY KEY (idCuentaFacebook)
);

CREATE TABLE Usuario (
	idUsuario serial,
	idTipoUsuario integer,
	idCuentaFacebook integer,
	nombre text,
	apellido text,
	usuario text,
	contrasena text,
	CONSTRAINT PK_Usuario PRIMARY KEY (idUsuario),
	CONSTRAINT FK_Usuario_TipoUsuario FOREIGN KEY (idTipoUsuario) REFERENCES TipoUsuario(idTipoUsuario),
	CONSTRAINT FK_Usuario_CuentaFacebook FOREIGN KEY (idCuentaFacebook) REFERENCES CuentaFacebook(idCuentaFacebook)
);

CREATE TABLE FollowUsuario (
	idFollow serial,
	idUsuarioFollower integer,
	idUsuarioFollowed integer,
	estadoSolicitud text,
	fechaRegistroSolicitud timestamp,
	fechaRespuestaSolicitud timestamp,
	CONSTRAINT PK_Follow PRIMARY KEY (idFollow),
	CONSTRAINT FK_Follow_UsuarioFollower FOREIGN KEY (idUsuarioFollower) REFERENCES Usuario(idUsuario),
	CONSTRAINT FK_Follow_UsuarioFollowed FOREIGN KEY (idUsuarioFollowed) REFERENCES Usuario(idUsuario)
);

CREATE TABLE VideoLlamada (
	idVideoLlamada serial,
	idUsuario integer,
	fechaInicio timestamp,
	fechaFin timestamp,
	plataforma text,
	longitud real,
	latitud real,
	CONSTRAINT PK_VideoLlamada PRIMARY KEY (idVideoLlamada),
	CONSTRAINT FK_VideoLlamada_Usuario FOREIGN KEY (idUsuario) REFERENCES Usuario(idUsuario)
);

CREATE TABLE Ubicacion (
	idUbicacion serial,
	nombre text, 
	abreviacion TEXT,
	CONSTRAINT PK_Ubicacion PRIMARY KEY(idUbicacion)
);

CREATE TABLE Nodo (
	idNodo serial,
	idUbicacion integer,
	edificio integer,
	nombre text,
	activo integer,
	CONSTRAINT PK_Nodo PRIMARY KEY (idNodo),
	CONSTRAINT FK_Nodo_Ubicacion FOREIGN KEY (idUbicacion) REFERENCES Ubicacion(idUbicacion)
);

CREATE TABLE CoordenadaNodo (
	idCoordenadaNodo serial,
	idNodo integer,
	longitud real,
	latitud real,
	CONSTRAINT PK_CoordenadaNodo PRIMARY KEY (idCoordenadaNodo),
	CONSTRAINT FK_CoordenadaNodo_Nodo FOREIGN KEY (idNodo) REFERENCES Nodo(idNodo)
);

CREATE TABLE Neighbor (
	idNeighbor serial,
	idNodo integer,
	idNodoNeighbor integer,
	CONSTRAINT PK_Neighbor PRIMARY KEY (idNeighbor),
	CONSTRAINT FK_Neighbor_Nodo FOREIGN KEY (idNodo) REFERENCES Nodo(idNodo),
	CONSTRAINT FK_Neighbor_NodoNeighbor FOREIGN KEY (idNodoNeighbor) REFERENCES Nodo(idNodo)
);

CREATE TABLE LocalizacionUsuario (
	idLocalizacionUsuario serial,
	idUsuario integer,
	idNodo integer,
	fechaLocalizacion timestamp,
	CONSTRAINT PK_LocalizacionUsuario PRIMARY KEY (idLocalizacionUsuario),
	CONSTRAINT FK_LocalizacionUsuario_Usuario FOREIGN KEY (idUsuario) REFERENCES Usuario(idUsuario),
	CONSTRAINT FK_LocalizacionUsuario_Localizacion FOREIGN KEY (idNodo) REFERENCES Nodo(idNodo)
);

CREATE TABLE Tour (
	idTour serial,
	idUsuario integer,
	nombreTour text,
	fechaCreacion timestamp,
	fechaInicio timestamp,
	fechaFin timestamp,
	CONSTRAINT PK_Tour PRIMARY KEY (idTour),
	CONSTRAINT FK_Tour_Usuario FOREIGN KEY (idUsuario) REFERENCES Usuario(idUsuario)
);

CREATE TABLE UsuarioTour (
	idUsuarioTour serial,
	idUsuario integer,
	idTour integer,
	estadoUsuarioTour text,
	fechaInicio timestamp,
	fechaFin timestamp,
	CONSTRAINT PK_UsuarioTour PRIMARY KEY (idUsuarioTour),
	CONSTRAINT FK_UsuarioTour_Usuario FOREIGN KEY (idUsuario) REFERENCES Usuario(idUsuario),
	CONSTRAINT FK_UsuarioTour_Tour FOREIGN KEY (idTour) REFERENCES Tour(idTour)
);

CREATE TABLE PuntoReunionTour (
	idPuntoReunion serial,
	idNodo integer,
	idTour integer,
	secuenciaPuntoReunion integer,
	CONSTRAINT PK_PuntoReunionTour PRIMARY KEY (idPuntoReunion),
	CONSTRAINT FK_PuntoReunionTour_Nodo FOREIGN KEY (idNodo) REFERENCES Nodo(idNodo),
	CONSTRAINT FK_PuntoReunionTour_Tour FOREIGN KEY (idTour) REFERENCES Tour(idTour)
);

CREATE TABLE DetalleUsuarioTour (
	idDetalleUsuarioTour serial,
	idUsuarioTour integer,
	idPuntoReunion integer,
	estado text,
	fechaInicio timestamp,
	fechaLlegada timestamp,
	CONSTRAINT PK_DetalleUsuarioTour PRIMARY KEY (idDetalleUsuarioTour),
	CONSTRAINT FK_DetalleUsuarioTour_UsuarioTour FOREIGN KEY (idUsuarioTour) REFERENCES UsuarioTour(idUsuarioTour),
	CONSTRAINT FK_DetalleUsuarioTour_PuntoReunionTour FOREIGN KEY (idPuntoReunion) REFERENCES PuntoReunionTour(idPuntoReunion)
);
--DROP TABLE
/*
DROP TABLE DetalleUsuarioTour;
DROP TABLE PuntoReunionTour;
DROP TABLE UsuarioTour;
DROP TABLE Tour;
DROP TABLE Neighbor;
DROP TABLE CoordenadaNodo;
DROP TABLE LocalizacionUsuario;
DROP TABLE Nodo;
DROP TABLE Ubicacion;
DROP TABLE VideoLlamada;
DROP TABLE FollowUsuario;
DROP TABLE Usuario;
DROP TABLE CuentaFacebook;
DROP TABLE TipoUsuario;
*/