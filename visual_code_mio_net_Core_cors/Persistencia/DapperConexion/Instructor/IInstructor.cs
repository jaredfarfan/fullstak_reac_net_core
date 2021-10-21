using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Persistencia.DapperConexion.Instructor
{
    //interface que representa las operaciones que quiero 
    //realizar alas tablas relacionadas a instructor
    public interface IInstructor
    {
        Task<IEnumerable<InstructorModel>> ObtenerLista();
        Task<InstructorModel> ObtenerPorId(Guid id);
        Task<int> Nuevo(string nombre, string apellidos, string titulo);
        //es int por la lita de transacciones
        Task<int> Actualiza(Guid instructorId,string nombre,string apellidos,string titulo);
        //con task se vuelve asyncrono ??
        Task<int> Elimina(Guid id);
    }
}