using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dominio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Persistencia;

namespace WebAPI.Controllers
{
    //base de endpoint
    //http://localhost:5000/WeatherForecast
    //http://localhost:5000/[controller]

    //esto trabaja  bajo notaciones
    [ApiController]
    //notacion tipo ruta/controller se remplaza por el nombre de la clase sin el "controller"
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly CursosOnlineContext context;
        //este objeto va ser cargado por el sevicio de webpi
        //acabamos de hacer inyeccion de dependiencia, crear un objeto indirectamente usando un servicio externo dentro de una clase
        public WeatherForecastController(CursosOnlineContext _context){
            context= _context;
        }
        //si quiere consumir esto el cliente tiene que ser tipo get
       [HttpGet]
        public IEnumerable<Curso> Get(){
            return context.Curso.ToList();
        }
    }
}
