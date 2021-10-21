create table Curso(
CursoId INT PRIMARY KEY IDENTITY,
Titulo varchar(300),
Descripcion varchar(300),
FechaPublicacion datetime,
FotoPortada varbinary(Max)
)


create table Precio(
PrecioId INT PRIMARY KEY IDENTITY,
PrecioActual money,
Promocion money,
CursoId int
)

alter table Precio
add CONSTRAINT FK_PRECIO_CURSO 
FOREIGN KEY (CursoId) REFERENCES Curso(CursoId)



create table Comentario(
ComentarioId INT PRIMARY KEY IDENTITY,
Alumnon varchar(500),
Puntaje int,
Comentario varchar(MAX),
CursoId int
)

alter table Comentari
add CONSTRAINT FK_COMENTARIO_CURSO 
FOREIGN KEY (CursoId) REFERENCES Curso(CursoId)


create table Instructor(
InstructorId INT PRIMARY KEY IDENTITY,
Nombre varchar(300),
Apellidos int,
Grado varchar(500),
Foto varbinary(Max)
)

--create table CursoInstructor(
--CursoId int PRIMARY KEY,
--InstructorId int PRIMARY KEY
--)
--esto desde el managment, los dos se ponen como clave primaria, seleccionando los dos, click opcion derecha mause
--luego esto siguietne
alter table CursoInstructor
add CONSTRAINT FK_CURSO_INTRUCTOR_CURSOID
FOREIGN KEY (CursoId) REFERENCES Curso(CursoId)


alter table CursoInstructor
add CONSTRAINT FK_CURSO_INSCTRUCTOR_INSTRUCTORID
FOREIGN KEY (InstructorId) REFERENCES Instructor(InstructorId)