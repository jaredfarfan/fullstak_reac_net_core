using System.Linq;
using System.Threading.Tasks;
using Dominio;
using Microsoft.AspNetCore.Identity;

namespace Persistencia
{
    public class DataPrueba
    {
        public async static Task InsertarData(CursosOnlineContext context,UserManager<Usuario> usuarioManager){
            //validar que exista un usurio en core identity dentro de la base de datos
            if(!usuarioManager.Users.Any()){
                var usuario = new Usuario{
                    NombreCompleto = "Jared Farfan",
                    UserName = "jared.farfan",
                    Email = "jared@gmail.com"
                };
                await usuarioManager.CreateAsync(usuario,"12345678Pa$");
            }
        }
    }
}