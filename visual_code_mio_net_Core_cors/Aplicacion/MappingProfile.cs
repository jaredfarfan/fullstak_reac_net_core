using System.Linq;
using Aplicacion.Cursos;
using AutoMapper;
using Dominio;

namespace Aplicacion
{
    //va manejar los mapeos entre las clases entoty core con las clases dto
    public class MappingProfile : Profile
    {
        //quienes van a mapearse
        public MappingProfile(){
            CreateMap<Curso, CursoDto>()
            //que dto la lista de instructores que van alimentar el dto de donde va provenir -> mapfrom
            //lo de adentro de MapFrom represneta entoty core, a la table curso instructor
            ///forMember es del dto
            ///InstructoresLink represnta a la tabla insctucor curso entity core
            .ForMember(x => x.Instructores, y => y.MapFrom( z => z.InstructoresLink.Select( a => a.Instructor).ToList()) )
            .ForMember(x=> x.Comentarios , y=> y.MapFrom(z=>z.ComentarioLista))//obtener lista comentarios
            .ForMember(x => x.Precio, y => y.MapFrom(y => y.PrecioPromocion));//obtener lista precio
            CreateMap<CursoInstructor, CursoInstructorDto>(); 
            CreateMap<Instructor, InstructorDto>();
            CreateMap<Comentario, ComentarioDto>();
            CreateMap<Precio, PrecioDto>();
        }
    }
}