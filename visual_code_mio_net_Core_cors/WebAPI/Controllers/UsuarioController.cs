using System.Threading.Tasks;
using Aplicacion.Seguridad;
using Dominio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    //se pone para que todos puedad ejecutar este controller
    //con alllowanonymous se excluye de la configguracion que se puse en startup
    //ciertos métodos pueden indicar que sea de libre acceso como este con allowanonymus
    [AllowAnonymous]
    public class UsuarioController:MiControllerBase
    {
        //http://localhost:5000/api/Usuario/login
         [HttpPost("login")]

        public async Task<ActionResult<UsuarioData>> Login(Login.Ejecuta parametros){
            return await Mediator.Send(parametros);
            //esta invocando a handle de la clase Login.css de aplicacion
        }
        // http://localhost:5000/api/Usuario/registrar
        [HttpPost("registrar")]
        public async Task<ActionResult<UsuarioData>> Registrar(Registrar.Ejecuta parametros){
                return await Mediator.Send(parametros);
        }

        // http://localhost:5000/api/Usuario
        [HttpGet]
        public async Task<ActionResult<UsuarioData>> DevolverUsuario(){
            return await Mediator.Send(new UsuarioActual.Ejecutar());
        }

        [HttpPut]
        public async Task<ActionResult<UsuarioData>> Actualizar(UsuarioActualizar.Ejecuta parametros){
           return await Mediator.Send(parametros);     
        }

    }
}