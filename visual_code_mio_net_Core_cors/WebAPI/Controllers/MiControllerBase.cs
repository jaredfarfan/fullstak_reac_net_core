using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MiControllerBase : ControllerBase
    {
        private IMediator _mediator;
        //como es protegida los hijos van usasr el objeto mediador, es decir los que hereden de este
        protected IMediator Mediator => _mediator ?? (_mediator = HttpContext.RequestServices.GetService<IMediator>());
    }
}