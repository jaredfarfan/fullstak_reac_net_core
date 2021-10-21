using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Dominio;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Cursos
{
    public class Consulta
    {
        //clase que represente la lista de elementos que va retornar -----IRequest es de la libreria MediatR
        public class ListaCursos : IRequest<List<CursoDto>>{}

        //representa la logia de la operacion
        public class Manejador : IRequestHandler<ListaCursos, List<CursoDto>>
        {
            //instanacia de cursosOnlineContext
            private readonly CursosOnlineContext _context;
            //inyectar el interface 
            private readonly IMapper _mapper;
            public Manejador(CursosOnlineContext context,IMapper mapper){
                _context=context;
                //lo inyectamos
                _mapper = mapper;
            }
            //transaccion para devolver todos los cursos

            //agergar propidades, eso es request
            //dto= data tranfer object
            public async Task<List<CursoDto>> Handle(ListaCursos request, CancellationToken cancellationToken)
            {
                //transcciones de servidores son de ida y vuelta
                //devuleva la data tipo lista de un metodo asyncrono
                //InstructoresLink id ,ThenInclude is data
                //crear dto es una clase especial otientada a entregar data especifica a un cliente
                var cursos = await _context.Curso
                .Include(x=> x.ComentarioLista)
                .Include(x=> x.PrecioPromocion)
                .Include(x=> x.InstructoresLink)
                .ThenInclude(x=> x.Instructor).ToListAsync();
                
                //dato que va retornar de que calse a un dto y pasar la data que tiene mapear
                //origne cursso, destino crusodtp y pasar la data
                 var cursosDto = _mapper.Map<List<Curso>, List<CursoDto>>(cursos);



                //como creamos el dto no podemos devolver con la lista curso si no con cursodto
                //entonces tenemos que pasar a dto
                return cursosDto;
            }
        }
    }
}