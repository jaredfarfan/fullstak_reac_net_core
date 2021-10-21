using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Dominio;
using FluentValidation;
using MediatR;
using Persistencia;

namespace Aplicacion.Cursos
{
    public class Nuevo
    {
        public class Ejecuta : IRequest {   
            //[Required(ErrorMessage="Por favor ingrese un titulo")]       otro metodo es fluent  
            //public Guid? CursoId {get;set;}
            public string Titulo {get;set;}
            public string Descripcion {get;set;}
            public DateTime? FechaPublicacion {get;set;}
            public List<Guid> ListaInstructor {get;set;}
            public decimal Precio {get;set;}
            public decimal Promocion{get;set;}
            
        }

        //clase para la logica de validacion
        public class EjecutaValidacion : AbstractValidator<Ejecuta>{
            public EjecutaValidacion(){
                RuleFor(x=> x.Titulo).NotEmpty();
                RuleFor(x=> x.Descripcion).NotEmpty();
                RuleFor(x=> x.FechaPublicacion).NotEmpty();
            }
        }
        //Ejecuta es el encargado de alimenar esta clase, indicar que va trabajar usando la defincion de datos
        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly CursosOnlineContext _context;
            public Manejador(CursosOnlineContext context)
            {
                _context = context;
            }
             
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                  Guid _cursoId = Guid.NewGuid();
                
                 var curso = new Curso {
                   CursoId = _cursoId,
                   Titulo = request.Titulo,
                   Descripcion = request.Descripcion,
                   FechaPublicacion = request.FechaPublicacion,
                   FechaCreacion = DateTime.UtcNow//comparar con la ubicacion del cliente y convertir la fecha actual del cliente
               };

                _context.Curso.Add(curso);

                if(request.ListaInstructor!=null){
                    foreach(var id in request.ListaInstructor){
                        var cursoInstructor = new CursoInstructor{
                        CursoId = _cursoId,
                        InstructorId = id
                        };
                        _context.CursoInstructor.Add(cursoInstructor);
                    }
                }

                 /*agregar logica para insertar un precio del curso*/
                var precioEntidad = new Precio{
                    CursoId = _cursoId,
                    PrecioActual = request.Precio,
                    Promocion = request.Promocion,
                    PrecioId = Guid.NewGuid()
                };

                _context.Precio.Add(precioEntidad);
                
                var valor = await _context.SaveChangesAsync();
                //si el estado que va devolver es 0 = no se realizo nunguna operacion
                //si devuelve >=1 si se hizo la transaccion
                //2 = 2 transacciones etc
                if(valor >0){
                    return Unit.Value;
                }
                throw new Exception("No se puedo insertar el curso");
            }
        }
    }
}