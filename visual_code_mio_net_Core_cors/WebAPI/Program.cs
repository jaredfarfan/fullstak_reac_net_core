using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dominio;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Persistencia;

namespace WebAPI
{
    public class Program
    {
        //no se puede utilziar async por que es un método 
        public static void Main(string[] args)
        {
            //todo esto es para hacer la migracion del archivo que se creó en persitencia
            var hostserver = CreateHostBuilder(args).Build();
            using(var ambiente = hostserver.Services.CreateScope()){
                var services = ambiente.ServiceProvider;
                try
                {
                //para insert el dato de prueba
                var userManager = services.GetRequiredService<UserManager<Usuario>>();
                //
                var context = services.GetRequiredService<CursosOnlineContext>();
                context.Database.Migrate();    
                DataPrueba.InsertarData(context,userManager).Wait();
                }
                catch (Exception ex)
                {
                    var logging = services.GetRequiredService<ILogger<Program>>();
                    logging.LogError(ex,"Ocurrio un error en la migracion");
                }
            }
            hostserver.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
