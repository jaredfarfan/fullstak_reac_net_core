using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicacion.Contratos;
using Aplicacion.Cursos;
using AutoMapper;
using Dominio;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Persistencia;
using Persistencia.DapperConexion;
using Persistencia.DapperConexion.Instructor;
using Persistencia.DapperConexion.Paginacion;
using Seguridad;
using WebAPI.Middleware;

namespace WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy("corsApp", builder => {
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            }));

            services.AddDbContext<CursosOnlineContext>(opt => {
                opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddOptions();
            //configurar la clase que acabo de crear en ersisntecia en daper
            services.Configure<ConexionConfiguracion>(Configuration.GetSection("ConnectionStrings"));
            //agregar la configuracion del mediador.
            services.AddMediatR(typeof(Consulta.Manejador).Assembly);
            //se configura por el fuent validaciton
            //si no quieres poner [autorize] por endpoint por controller tienes que hacer esta configuración
            //todo lo que esta dentro del addcontrolllers()
            //igual teniendo esta configuracion por controller puedes poner el [AllowAnonymous] diciendo que se controller omitira esta confirg de autorizacion 
            services.AddControllers(opt =>{
                //agregar una nueva regla para nos controladores, tieen que estar aitenticado
                //para controles tenga autoriazion para procesar el reques de un cliente
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                opt.Filters.Add(new AuthorizeFilter(policy));
            })
            .AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<Nuevo>());

            #region configuracion de core identity en weba apy
            //variable que represent la instancia de usuario que viene desde persistencia dominio
            var builder = services.AddIdentityCore<Usuario>();
            var identityBuilde = new  IdentityBuilder(builder.UserType, builder.Services);
            //instanciando el objeto de rol manager
            //entidad que va reprensentar los roles
            identityBuilde.AddRoles<IdentityRole>();
            //necesitamos add data de los roles dentro de los tokens de seguridad
            //instanciar a los claims, agregar claims
            //dos entidades que van a trabajar entre usuario y rol
            identityBuilde.AddClaimsPrincipalFactory<UserClaimsPrincipalFactory<Usuario,IdentityRole>>();


            //anstancia de entity framework
            identityBuilde.AddEntityFrameworkStores<CursosOnlineContext>();
            
            //administrador de accesode vendra de singinmanager que va tomar los datos de clase usuario
            //manejar el acceso de usuarios
            identityBuilde.AddSignInManager<SignInManager<Usuario>>();


            //tengo una única instancia en este caso de la clase SystemClock en todo el ciclo de la aplicación?  si es así el .Net Core me evita hacer el típico "if instance != null return instance". 
            services.TryAddSingleton<ISystemClock,SystemClock>();
            #endregion

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Mi palabra secreta"));
            //logica para tener  autorizacion  
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt => {
            //parametros que va tener el token
            //se pueden poner validaciones como solo permitir de esta IP
                opt.TokenValidationParameters = new TokenValidationParameters{
                    //cualquier tipo de requet del cliente debe de ser validado
                    ValidateIssuerSigningKey = true,
                    //pasar la palabra clabe
                    IssuerSigningKey = key,
                    //quien va crear esos tokens false=global. Ciertas companias es avegriguar la ip publica y configurarlas
                    ValidateAudience = false,
                    //nosotros recibamos un pedido y enviemos ese toquen a ese ip lejano y desconocido
                    //es desconocido false= no trabaje
                    ValidateIssuer = false
                };
            });
            //inyecte services . addscopet, inyecte al interface
            //va ser posible acceder a los metodos que van a generar los tokets de seguridad
            services.AddScoped<IJwtGenerador, JwtGenerador>();
            //despues de poner la clase para obetner la "sesion" del usuario
            services.AddScoped<IUsuarioSesion, UsuarioSesion>();
            //se agrego para el automaper despues de añadir la libreia y hacer de esa clase, configurada el mapeo
            services.AddAutoMapper(typeof(Consulta.Manejador));
            //implementar el dapper
            services.AddTransient<IFactoryConnection, FactoryConnection>();
            //indicar el dependicy injectios de las operaciones
            services.AddScoped<IInstructor, InstructorRepositorio>();
            services.AddScoped<IPaginacion, PaginacionRepositorio>();
            //metodos para soportar swagger
            services.AddSwaggerGen( c => {
               c.SwaggerDoc("v1", new OpenApiInfo{
                Title = "Services para mantenimiento de cursos",
                Version = "v1"
               });   
               //AJUTE ADICIONAR PARA NO TENER conflicto con los endpoint  
               //para que tome los parametros (namespace) de la clase ejecuta
               c.CustomSchemaIds(c=>c.FullName);
            });
            //swagger api
            //objetivo es documentar los web services del proyecto, existen extenciones para probarlo
            //pero mejor es posman para hacer pruebas
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("corsApp");
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                //hemos creado nuestro propio midelware abajo esta

            }
            app.UseMiddleware<ManejadorErrorMiddleware>();

            //app.UseHttpsRedirection();
            //va utilizar la autentificacion jwt token
             app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSwagger();
            app.UseSwaggerUI(
                //EL ENDPOINT PARA LEER O CONSUMIR LA INFORMACION SSERA ESTE 
                 c=>{
                c.SwaggerEndpoint("/swagger/v1/swagger.json","Cursos Online v1");
                
            }
            );
            
        }
    }
}
