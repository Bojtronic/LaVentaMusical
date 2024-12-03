INSERT INTO Usuarios (Identificacion, Nombre_Usuario, Apellidos, Genero, Email, TipoTarjeta, NumeroTarjeta, Password, Perfil)
VALUES 
('1234567890', 'Juan', 'Pérez', 'M', 'juan.perez@example.com', 'Visa', '4111111111111111', 'pass123', 'admin'),
('0987654321', 'Ana', 'López', 'F', 'ana.lopez@example.com', 'MasterCard', '5500000000000004', 'pass456', 'usuario'),
('5678901234', 'Carlos', 'Gómez', 'M', 'carlos.gomez@example.com', NULL, NULL, 'pass789', 'usuario');


INSERT INTO Generos (Descripcion)
VALUES 
('Rock'),
('Pop'),
('Jazz');


INSERT INTO Artistas (Nombre_Artistico, Fecha_Nacimiento, Nombre_Real, Nacionalidad, Imagen_Artista)
VALUES 
('Queen', '1946-09-05', 'Freddie Mercury', 'Reino Unido', NULL),
('Madonna', '1958-08-16', 'Madonna Louise Ciccone', 'Estados Unidos', NULL),
('Miles Davis', '1926-05-26', 'Miles Dewey Davis III', 'Estados Unidos', NULL);


INSERT INTO Albumes (Id_Artista, Nombre_Album, Ano_Lanzamiento, Imagen_Album)
VALUES 
(1, 'A Night at the Opera', 1975, NULL),
(2, 'Like a Virgin', 1984, NULL),
(3, 'Kind of Blue', 1959, NULL);


INSERT INTO Canciones (Id_Genero, Id_Album, Nombre_Cancion, Video_URL, Precio, Canciones_Disponibles)
VALUES 
(1, 1, 'Bohemian Rhapsody', 'https://www.example.com/bohemian', 1.99, 100),
(2, 2, 'Material Girl', 'https://www.example.com/materialgirl', 1.49, 50),
(3, 3, 'So What', 'https://www.example.com/sowhat', 1.99, 75);


INSERT INTO Facturas (Id_Usuario, FechaCompra, NumeroFactura, SubTotal, IVA, Total)
VALUES 
(1, '2024-11-20', 'F001', 10.00, 1.60, 11.60),
(2, '2024-11-21', 'F002', 5.00, 0.80, 5.80),
(3, '2024-11-22', 'F003', 15.00, 2.40, 17.40);


INSERT INTO DetalleFactura (Id_Factura, Id_Cancion, Cantidad, PrecioUnitario, Subtotal)
VALUES 
(1, 1, 2, 1.99, 3.98),
(2, 2, 1, 1.49, 1.49),
(3, 3, 3, 1.99, 5.97);

INSERT INTO Pagos (Id_Factura, MetodoPago, MontoPagado, FechaPago, CodigoAutorizacion)
VALUES 
(1, 'Tarjeta', 11.60, '2024-11-20', 'AUTH001'),
(2, 'Tarjeta', 5.80, '2024-11-21', 'AUTH002'),
(3, 'Tarjeta', 17.40, '2024-11-22', 'AUTH003');


INSERT INTO CancionesFacturadas (Id_Factura, Id_Cancion)
VALUES 
(1, 1),
(2, 2),
(3, 3);


INSERT INTO Carritos (Id_Cancion, Id_Usuario)
VALUES 
(1, 1),
(2, 2),
(3, 3);

INSERT INTO Tipo_Tarjeta (Descripción_Tipo)
VALUES 
('Visa'),
('MasterCard'),
('Amex');


INSERT INTO Tarjetas (Id_Usuario, Id_Tipo, Numero_Tarjeta, Fecha_Expiracion, CVC)
VALUES 
(1, 1, '4111111111111111', '2026-12-31', 123),
(2, 2, '5500000000000004', '2025-11-30', 456),
(3, 3, '340000000000009', '2027-10-31', 789);


