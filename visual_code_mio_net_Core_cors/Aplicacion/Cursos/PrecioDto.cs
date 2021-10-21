using System;
using Dominio;

namespace Aplicacion.Cursos
{
    public class PrecioDto
    {
        public Guid PrecioId{get;set;}
        public decimal PrecioActual {get;set;}
        public decimal Promocion {get;set;}
        public Guid CursoId {get;set;}
    }
}