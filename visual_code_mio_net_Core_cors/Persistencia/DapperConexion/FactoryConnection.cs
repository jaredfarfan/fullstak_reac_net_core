using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Persistencia.DapperConexion
{
    //poderme conectar dinamicamente
    public class FactoryConnection : IFactoryConnection
    {

        private IDbConnection _connection;
        //tener acceso a cadena conexion que esta dentro de la propiedad
        //en statup le dijimos cual cadena conecion se va alimentar
        private readonly IOptions<ConexionConfiguracion> _configs;

        //inyecar el objeto dentro de otra casle con cosntructor
          public FactoryConnection(IOptions<ConexionConfiguracion> configs){
             _configs = configs;
        }
        public void CloseConnection()
        {
            if(_connection != null && _connection.State == ConnectionState.Open){
                _connection.Close();
            }
        }

        public IDbConnection GetConnection()
        {
             if(_connection == null){
                 //creame uno
                _connection = new SqlConnection(_configs.Value.DefaultConnection);
            }
            //estado de la cadena
            if(_connection.State != ConnectionState.Open){
                _connection.Open();
            }
            return _connection;
        }
    }
}