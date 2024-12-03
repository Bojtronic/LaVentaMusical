CREATE DATABASE LaVentaMusical;

USE LaVentaMusical;

-- Tabla Usuarios
CREATE TABLE Usuarios (
    Id_Usuario INT PRIMARY KEY IDENTITY,
    Identificacion VARCHAR(20) UNIQUE NOT NULL,
    Nombre_Usuario VARCHAR(50) NOT NULL,
    Apellidos VARCHAR(50) NOT NULL,
    Genero CHAR(1) CHECK (Genero IN ('M', 'F')),
    Email VARCHAR(100) UNIQUE NOT NULL,
    TipoTarjeta VARCHAR(20),
    NumeroTarjeta VARCHAR(19),
    Password VARCHAR(200) NOT NULL,
    Perfil VARCHAR(20) DEFAULT 'usuario'
);

-- Tabla Géneros
CREATE TABLE Generos (
    Id_Genero INT PRIMARY KEY IDENTITY,
    Descripcion VARCHAR(100) NOT NULL
);

-- Tabla Artistas
CREATE TABLE Artistas (
    Id_Artista INT PRIMARY KEY IDENTITY,
    Nombre_Artistico VARCHAR(100) NOT NULL,
    Fecha_Nacimiento DATE,
    Nombre_Real VARCHAR(100),
    Nacionalidad VARCHAR(50),
    Imagen_Artista VARBINARY(MAX)
);

-- Tabla Álbumes
CREATE TABLE Albumes (
    Id_Album INT PRIMARY KEY IDENTITY,
    Id_Artista INT NOT NULL FOREIGN KEY REFERENCES Artistas(Id_Artista),
    Nombre_Album VARCHAR(100) NOT NULL,
    Ano_Lanzamiento INT,
    Imagen_Album VARBINARY(MAX)
);

-- Tabla Canciones
CREATE TABLE Canciones (
    Id_Cancion INT PRIMARY KEY IDENTITY,
    Id_Genero INT NOT NULL FOREIGN KEY REFERENCES Generos(Id_Genero),
    Id_Album INT NOT NULL FOREIGN KEY REFERENCES Albumes(Id_Album),
    Nombre_Cancion VARCHAR(100) NOT NULL,
    Video_URL VARCHAR(255),
    Precio DECIMAL(10, 2),
    Canciones_Disponibles INT DEFAULT 0
);

-- Tabla Facturas
CREATE TABLE Facturas (
    Id_Factura INT PRIMARY KEY IDENTITY,
    Id_Usuario INT NOT NULL FOREIGN KEY REFERENCES Usuarios(Id_Usuario),
    FechaCompra DATETIME DEFAULT GETDATE(),
    NumeroFactura VARCHAR(20) UNIQUE,
    SubTotal DECIMAL(10, 2),
    IVA DECIMAL(10, 2),
    Total DECIMAL(10, 2) NOT NULL
);

-- Tabla Detalle Factura
CREATE TABLE DetalleFactura (
    DetalleId INT PRIMARY KEY IDENTITY,
    Id_Factura INT NOT NULL FOREIGN KEY REFERENCES Facturas(Id_Factura),
    Id_Cancion INT NOT NULL FOREIGN KEY REFERENCES Canciones(Id_Cancion),
    Cantidad INT,
    PrecioUnitario DECIMAL(10, 2),
    Subtotal DECIMAL(10, 2)
);

-- Tabla Pagos
CREATE TABLE Pagos (
    PagoId INT PRIMARY KEY IDENTITY,
    Id_Factura INT NOT NULL FOREIGN KEY REFERENCES Facturas(Id_Factura),
    MetodoPago VARCHAR(50),
    MontoPagado DECIMAL(10, 2),
    FechaPago DATETIME DEFAULT GETDATE(),
    CodigoAutorizacion VARCHAR(50)
);

-- Tabla Canciones Facturadas
CREATE TABLE CancionesFacturadas (
    Id_Factura INT NOT NULL FOREIGN KEY REFERENCES Facturas(Id_Factura),
    Id_Cancion INT NOT NULL FOREIGN KEY REFERENCES Canciones(Id_Cancion),
    PRIMARY KEY (Id_Factura, Id_Cancion)
);

-- Tabla Carritos
CREATE TABLE Carritos (
    Id_Cancion INT NOT NULL FOREIGN KEY REFERENCES Canciones(Id_Cancion),
    Id_Usuario INT NOT NULL FOREIGN KEY REFERENCES Usuarios(Id_Usuario),
    PRIMARY KEY (Id_Cancion, Id_Usuario)
);

-- Tabla Tarjetas
CREATE TABLE Tarjetas (
    Id_Tarjeta INT PRIMARY KEY IDENTITY,
    Id_Usuario INT NOT NULL FOREIGN KEY REFERENCES Usuarios(Id_Usuario),
    Id_Tipo INT NOT NULL,
    Numero_Tarjeta VARCHAR(19),
    Fecha_Expiracion DATE,
    CVC INT
);

-- Tabla Tipo_Tarjeta
CREATE TABLE Tipo_Tarjeta (
    Id_Tipo INT PRIMARY KEY IDENTITY,
    Descripción_Tipo VARCHAR(50) NOT NULL
);

-- Establecer la relación entre Tarjetas e Tipo_Tarjeta
ALTER TABLE Tarjetas
ADD FOREIGN KEY (Id_Tipo) REFERENCES Tipo_Tarjeta(Id_Tipo);
