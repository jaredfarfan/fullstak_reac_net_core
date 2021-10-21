using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Persistencia.DapperConexion.Instructor;

namespace Aplicacion.Instructores
{
    public class Consulta
    {
        //si se de entity framwork o dapper crear dos calses
        //
        public class Lista : IRequest<List<InstructorModel>> {
                //parametors no hay
        }
        public class Manejador : IRequestHandler<Lista, List<InstructorModel>>
        {
            //quien tiene que trabajar con la logia de procedimientos almacenados
            private readonly IInstructor _instructorRepository;
            public Manejador(IInstructor instructorRepository){
                   _instructorRepository = instructorRepository; 
            }
     

            public async Task<List<InstructorModel>> Handle(Lista request, CancellationToken cancellationToken)
            {
                  var resultado = await _instructorRepository.ObtenerLista();
                  return resultado.ToList();
                 
            }
        }

    }
}