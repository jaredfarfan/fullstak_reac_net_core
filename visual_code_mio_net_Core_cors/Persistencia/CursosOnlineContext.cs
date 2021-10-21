using Dominio;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistencia
{
    //definir que se un servicio dentro de webapi
    public class CursosOnlineContext : IdentityDbContext<Usuario>//este se utliza por el core identity
    {
        public CursosOnlineContext(DbContextOptions options) :base(options){

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder){
            //crear el archivo de migracion de la logica de la tablas de core identity y las otras clases
            base.OnModelCreating(modelBuilder);
            //este es foreingkey, esta tiene un key compuesta/ dos culumnas que tienen una clave primaria
            modelBuilder.Entity<CursoInstructor>().HasKey(ci=> new {ci.InstructorId, ci.CursoId});
        }

        //representacion de la base de datos instanaciada por entity framework
        public DbSet<Comentario> Comentario {get;set;}
        public DbSet<Curso> Curso {get;set;}
        public DbSet<CursoInstructor> CursoInstructor {get;set;}
        public DbSet<Instructor> Instructor {get;set;}
        public DbSet<Precio> Precio {get;set;}
        public DbSet<Documento> Documento { get;set; }
        //public DbSet<Usuario> Usuario {get;set;}




    }
}