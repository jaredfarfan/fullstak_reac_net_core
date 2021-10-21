using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aplicacion.Cursos;
using Dominio;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistencia.DapperConexion.Paginacion;

namespace WebAPI.Controllers
{
    ///http://localhost:5000/api/Cursos
    [Route("api/[controller]")]
    [ApiController]
    public class CursosController:MiControllerBase
    {
        //imediator para invocar el poyeto applicacion
        //este fue para poner uno por uno en cada controler , se creo micontrollerbase
        /*public readonly IMediator _mediator;
        public CursosController(IMediator mediador){
            _mediator = mediador;
        }*/
        
        [HttpGet]
        //[Authorize]
        //se comento porque el el startup se modiifco para que sea general para todos los controladores
        public async Task<ActionResult<List<CursoDto>>> Get(){
            return await Mediator.Send(new Consulta.ListaCursos());
        }
        //http://localhost:5000/api/Cursos/{id}
        //http://localhost:5000/api/Cursos/1
        //tipo de parametro que va tener el endpoint
        [HttpGet("{id}")]
        public async Task<ActionResult<CursoDto>> Detalle(Guid id){
            return await Mediator.Send(new ConsultaId.CursoUnico{Id= id});
        }
        [HttpPost]
        public async Task<ActionResult<Unit>> Crear(Nuevo.Ejecuta data){
            return await Mediator.Send(data);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Editar(Guid id, Editar.Ejecuta data){
            data.CursoId = id;
            return await Mediator.Send(data);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Eliminar(Guid id){
            return await Mediator.Send(new Eliminar.Ejecuta{Id=id});
        }
        
        [HttpPost("report")]
        public async Task<ActionResult<PaginacionModel>> Report(PaginacionCurso.Ejecuta data){
            return await Mediator.Send(data);
        }
    }
}