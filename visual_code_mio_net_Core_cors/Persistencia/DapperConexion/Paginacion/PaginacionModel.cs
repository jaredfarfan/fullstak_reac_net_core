using System.Collections.Generic;

namespace Persistencia.DapperConexion.Paginacion
{
    public class PaginacionModel
    {
        //lista recors //data generica retornará
        //se convierta a un json va retornar
        //[{cursoId:"1234","titulo":"aspenet"},{cursoId:"444","titulo":"react"}] tipo Idictionary tipo json 
        public List<IDictionary<string,object>> ListaRecords{get;set;}
        public int TotalRecors {get;set;}
        public int NumeroPaginas{get;set;}

        
    }
}