CREATE PROCEDURE usp_obtener_curso_paginacion(
@NombreCurso nvarchar(500),
@Ordenamiento nvarchar(500),
@NumeroPagina int,
@CantidadElementos int,
@TotalRecords int output,
@TotalPaginas int output
) as
BEGIN
	--NO DEVUELVA EL NUMERO DE TRANSACCIONES
	SET NOCOUNT ON
	--EVITAR CONFLICTOS CUANDO SE REALICEN ESTA CONSULTA.. NO BLOQUEE SI USUARIO ESTA TRATANDO ACTUALIZAR LA DATA DE CURSO, 
	--ME HAGA ESPERAR DE MI CONSULTA
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	--INICIO Y FINAL
	DECLARE @Inicio int
	DECLARE @Fin int

	if @NumeroPagina =1
		BEGIN 
			SET @Inicio = (@NumeroPagina * @CantidadElementos) - @CantidadElementos
			SET @Fin = @NumeroPagina * @CantidadElementos
		END
	ELSE
	BEGIN 
		SET @Inicio = ((@NumeroPagina*@CantidadElementos) - @CantidadElementos) + 1;
		SET @Fin = @NumeroPagina * @CantidadElementos
	END

	---TABLA TEMPORAR QUE ALMACENE LOS DATOS ANTES QUE SE ENTREGE AL CLIENTE
	-- CUANDO SE PONE # LE DECIMOS QUE ES TOMPORAL, SOLO CUANDO SE EJECUTA EL SP
	CREATE TABLE #TMP(
	rowNumber int IDENTITY(1,1),
	ID UNIQUEIDENTIFIER
	)
	--VARIABLE DEL QUERY
	DECLARE @SQL NVARCHAR(MAX)
	SET @SQL = 'SELECT CursoId FROM Curso'

	if @NombreCurso is not null
		BEGIN 
			SET @SQL = @SQL + ' WHERE Titulo LIKE ''%'+ @NombreCurso+'%'' '
		END

	IF @Ordenamiento IS NOT NULL
		BEGIN 
			SET @SQL = @SQL + ' ORDER BY ' + @Ordenamiento
		END

		--select CursoId From Curso Where Titulo like '% ASP %' ORDER BY Titulo
		--TABLA TEMPORAR VA TENER EL ID DE CURSOS
		insert into #TMP(ID)
		exec sp_executesql @SQL

		SELECT @TotalRecords = COUNT(*) FROM #TMP

		IF @TotalRecords > @CantidadElementos
			BEGIN
				SET @TotalPaginas = @TotalRecords / @CantidadElementos
				IF (@TotalRecords % @CantidadElementos) > 0 
					BEGIN
						SET @TotalPaginas = @TotalPaginas + 1
					END
			END
		ELSE
			BEGIN
				SET @TotalPaginas =1;
			END


		SELECT c.CursoId,c.Titulo,c.Descripcion,c.FechaPublicacion,c.FotoPortada,c.FechaCreacion, p.PrecioActual, p.Promocion
		FROM #TMP t inner join Curso c on c.CursoId = t.ID
		left join Precio p on c.CursoId = p.CursoId
		where t.rowNumber>= @Inicio and t.rowNumber <=@Fin


END