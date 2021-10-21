using System.Data;

namespace Persistencia.DapperConexion
{
    public interface IFactoryConnection
    {
        //cerrar las conexiones existentes
        void CloseConnection();
        //devolver el objeto de conexion
         IDbConnection GetConnection();
    }
}