using System;

namespace Persistencia.DapperConexion.Instructor
{
    ///clase que representa la data que va retornar
    public class InstructorModel
    {
        //LOS NOMBRES TIENEN QUE SER LOS MISMOS QUE SE PONEN AL PROCEDIMIENTO ALMACENADO
        public Guid InstructorId{get;set;}
        public string Nombre{get;set;}
        public string Apellidos {get;set;}
        public string Titulo {get;set;}
        public DateTime? FechaCreacion {get;set;}
    }
}