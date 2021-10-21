using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;

namespace Persistencia.DapperConexion.Instructor
{
    public class InstructorRepositorio : IInstructor
    {
        //varibale que represente la factoria de coneccion
        private readonly IFactoryConnection _factoryConnection;
        //lo inyectamos
        public InstructorRepositorio(IFactoryConnection factoryConnection)
        {
            _factoryConnection = factoryConnection;
        }
        public async Task<int> Actualiza(Guid instructorId,string nombre,string apellidos,string titulo)
        {
            var storeProcedure = "usp_instructor_editar";
            try
            {
                var connection = _factoryConnection.GetConnection();
                //los parametors se llamaran de esa forma
                var resultado = await connection.ExecuteAsync(
                    storeProcedure,
                    new {
                        InstructorId = instructorId,
                        Nombre = nombre,
                        Apellidos = apellidos,
                        Titulo = titulo
                    },
                    commandType:CommandType.StoredProcedure
                );

                _factoryConnection.CloseConnection();
                return resultado;

            }
            catch (System.Exception e)
            {
                
                throw new Exception("No se pudo editar la data",e);
            }
        }

        public async Task<int> Elimina(Guid id)
        {
            var storeProcedure = "usp_instructor_elimina";
            try
            {
                var connection = _factoryConnection.GetConnection();
                var resultado = await connection.ExecuteAsync(
                    storeProcedure,
                    new{
                        InstructorId = id
                    },
                    commandType:CommandType.StoredProcedure
                );
                _factoryConnection.CloseConnection();
                return resultado;
            }
            catch (System.Exception e)
            {
                throw new Exception("No se pudo eliminar el instructor",e);
            }
        }

        public async Task<int> Nuevo(string nombre, string apellidos, string titulo)
        {
            var storeProcedure = "usp_instructor_nuevo";
            try
            {
                var connection = _factoryConnection.GetConnection();
                var resultado = await connection.ExecuteAsync(
                storeProcedure,
                new
                {
                    InstructorId = Guid.NewGuid(),
                    Nombre = nombre,
                    Apellidos = apellidos,
                    Titulo = titulo
                },
                commandType: CommandType.StoredProcedure
                );
                 _factoryConnection.CloseConnection();

                return resultado;
            }
            catch (System.Exception e)
            {
                
                throw new Exception("No se pudo guardar el nuevo insctructor",e);
            }
            
        }

        public async Task<IEnumerable<InstructorModel>> ObtenerLista()
        {
            //objeto que contendra la lista de resultados
            ///no se puede instanciar objetos de interfaces
            IEnumerable<InstructorModel> instructorList = null;
            //nombre del procedimiento almacenado
            var storeProcedure = "usp_Obtener_Instructores";
            try
            {
                var connection = _factoryConnection.GetConnection();
                //querys de tipo asyncrono
                instructorList = await connection.QueryAsync<InstructorModel/*Tipo de data que quiero retorje*/>(storeProcedure/*nombre store procedure*/, null/*parametors*/, commandType: CommandType.StoredProcedure);

            }
            catch (Exception e)
            {
                throw new Exception("Error en la consulta de datos", e);
            }
            finally
            {
                _factoryConnection.CloseConnection();
            }
            return instructorList;
        }

        public async Task<InstructorModel> ObtenerPorId(Guid id)
        {
            var storeProcedure = "usp_obtener_instructor_por_id";
            InstructorModel instructor = null;
            try{
                var connection  = _factoryConnection.GetConnection();
                instructor = await connection.QueryFirstAsync<InstructorModel>(
                    storeProcedure,
                    new {
                        Id = id
                    },
                    commandType : CommandType.StoredProcedure
                );

                return instructor;

            }catch(Exception e){
                throw new Exception("No se pudo encontrar el instructor",e);
            }
        }
    }
}