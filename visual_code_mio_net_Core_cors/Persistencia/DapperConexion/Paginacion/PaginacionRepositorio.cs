using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using System.Linq;

namespace Persistencia.DapperConexion.Paginacion
{
    public class PaginacionRepositorio : IPaginacion
    {
        //importar el objeto de tipo coneccion
        //inyecatr factory ocnetion porque estamos con dapper
        private readonly IFactoryConnection _factoryConnection;
        List<IDictionary<string,object>> listaReporte = null;
        public PaginacionRepositorio(IFactoryConnection factoryConnection){
            _factoryConnection = factoryConnection;
        }
        public async Task<PaginacionModel> devolverPaginacion(string storeProcedure, int numeroPagina, int cantidadElementos, IDictionary<string, object> parametrosFitro, string ordenamientoColumna)
        {
            PaginacionModel paginacionModel = new PaginacionModel();
            int totalRecords=0;
            int totalPaginas =0;
            try
            {
                var conection = _factoryConnection.GetConnection();
                DynamicParameters parametros = new DynamicParameters();

                foreach (var item in parametrosFitro)
                {
                    parametros.Add("@"+item.Key,item.Value);   
                }
                //parametros de entrada
                parametros.Add("@NumeroPagina",numeroPagina);
                parametros.Add("@CantidadElementos",cantidadElementos);
                parametros.Add("@Ordenamiento",ordenamientoColumna);

                //parametros de salida
                parametros.Add("@TotalRecords",totalRecords,DbType.Int32,ParameterDirection.Output);
                parametros.Add("@TotalPaginas",totalPaginas,DbType.Int32,ParameterDirection.Output);

                var result= await conection.QueryAsync(storeProcedure,parametros,commandType:CommandType.StoredProcedure);
                //la data que devuelve es ienumerable, convertir a tipo idictionary
                //cada resigistro se convertia en un tipo dictiorary, el x represneta un registro que viene de la tabla, se va parciar para convert a un tipo dictionary
                listaReporte = result.Select(x=> (IDictionary<string,object>)x).ToList();
                paginacionModel.ListaRecords = listaReporte;

                paginacionModel.NumeroPaginas = parametros.Get<int>("@TotalPaginas");
                paginacionModel.TotalRecors = parametros.Get<int>("@TotalRecords");
                
            }
            catch (System.Exception e)
            {
                
                throw new System.Exception("No se puedo ejecutar el procedimiento alamacenado" + e.ToString());
            }
            finally{
                _factoryConnection.CloseConnection();
            }
            return paginacionModel;
        }
    }
}