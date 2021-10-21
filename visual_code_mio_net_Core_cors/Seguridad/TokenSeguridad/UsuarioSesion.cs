using System.Linq;
using System.Security.Claims;
using Aplicacion.Contratos;
using Microsoft.AspNetCore.Http;

namespace Seguridad
{
    public class UsuarioSesion : IUsuarioSesion
    {
        //para poder tener acceso al usuario que esta en "sesion"
        private readonly IHttpContextAccessor _httpContextAccessor;
        //inyectar en usuario sesion
        public UsuarioSesion(IHttpContextAccessor httpContextAccessor){
            _httpContextAccessor = httpContextAccessor;
        }
        //leugo configurar en startup
        public string ObtenerUsuarioSesion()
        {
            //?=puede ser nulo . data del core identity user se le llama claims
            var userName = _httpContextAccessor.HttpContext.User?.Claims?.FirstOrDefault(x => x.Type==ClaimTypes.NameIdentifier)?.Value;
            return userName;
        }
    }
}