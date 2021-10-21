create procedure usp_instructor_nuevo(
@InstructorId uniqueidentifier,
@Nombre nvarchar(500),
@Apellidos nvarchar(500),
@Titulo nvarchar(500)
)
as
	 begin
		insert into Instructor(InstructorId, NombreInstructor, Apellidos,Grado)
		values(@InstructorId,@Nombre,@Apellidos,@Titulo)
	 end

	 create procedure usp_instructor_editar(
	 @InstructorId uniqueidentifier,
@Nombre nvarchar(500),
@Apellidos nvarchar(500),
@Titulo nvarchar(500)
	 )
	 as
	 begin 
	 update Instructor 
	 set 
	 NombreInstructor = @Nombre, Apellidos = @Apellidos, Grado=@Titulo,fechaCreacion= GETUTCDATE()
	 where InstructorId = @InstructorId
	 end

	 alter procedure usp_obtener_instructor_por_id
	 (
	 @id uniqueidentifier
	 )
	 as
	 begin 
	 select instructorId,NombreInstructor Nombre, Apellidos,Grado Titulo,FechaCreacion
	 from Instructor where InstructorId = @id
	 end

	 select * from Instructor


	 ----

	 luego se agrega el fechacreacion
	 USE [CursosOnline]
GO
/****** Object:  StoredProcedure [dbo].[usp_instructor_nuevo]    Script Date: 25/10/2020 11:43:59 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[usp_instructor_nuevo](
@InstructorId uniqueidentifier,
@Nombre nvarchar(500),
@Apellidos nvarchar(500),
@Titulo nvarchar(500)
)
as
	 begin
		insert into Instructor(InstructorId, NombreInstructor, Apellidos,Grado,FechaCreacion)
		values(@InstructorId,@Nombre,@Apellidos,@Titulo,GETUTCDATE())
	 end


create procedure usp_obtener_instructores
	 as
	 begin 
	 select instructorId,NombreInstructor Nombre, Apellidos,Grado Titulo,fechaCreacion
	 from Instructor 
	 end