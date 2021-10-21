using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using AutoMapper;
using Dominio;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Cursos
{
    public class ConsultaId
    {
        //clase que represente los datos que va retornar
        public class CursoUnico : IRequest<CursoDto>{
            public Guid Id {get;set;}
        }

        //representa la logia de la operacion
        public class Manejador : IRequestHandler<CursoUnico, CursoDto>
        {
            //instanacia de cursosOnlineContext
            private readonly CursosOnlineContext _context;
            private readonly IMapper _mapper;
            public Manejador(CursosOnlineContext context,IMapper mapper){
                _context=context;
                _mapper = mapper;
            }

            public async Task<CursoDto> Handle(CursoUnico request, CancellationToken cancellationToken)
            {
                var curso = await _context.Curso
                .Include(x=>x.ComentarioLista)
                .Include(x=>x.PrecioPromocion)
                .Include(x=> x.InstructoresLink)//union con el instructor
                .ThenInclude(y=>y.Instructor)//insclur los datos
                .FirstOrDefaultAsync(a=> a.CursoId == request.Id);
                 if(curso==null){
//                    throw new Exception("El curso no existe");
                throw new ManejadorExcepcion(HttpStatusCode.NotFound, new {mensaje = "No se encontró el curso"});
                }

                var cursoDto = _mapper.Map<Curso, CursoDto>(curso);
                return cursoDto;
                
            }
            //transaccion para devolver todos los cursos

            //agergar propidades, eso es request

        }
    }
}