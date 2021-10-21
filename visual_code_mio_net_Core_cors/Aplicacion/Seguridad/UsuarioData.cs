namespace Aplicacion.Seguridad
{
    //va representar la data que quiero devolver al cliente
    //para no devolver todos los datos
    public class UsuarioData
    {
         public string NombreCompleto {get;set;}
        public string Token {get;set;}
        public string Email {get;set;}
        public string Username {get;set;}
        public string Imagen {get;set;}
        public ImagenGeneral ImagenPerfil { get; set; }
    }
}