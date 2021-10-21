using System;
using System.Net;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace WebAPI.Middleware
{
    public class ManejadorErrorMiddleware
    {
        private readonly RequestDelegate Next;
        private readonly ILogger<ManejadorErrorMiddleware> Logger;
        public ManejadorErrorMiddleware(RequestDelegate next, ILogger<ManejadorErrorMiddleware> logger){
            Next = next;
            Logger = logger;
        }
        //dentro de context están los parametros toda la data que el usuario requiere
        public async Task Invoke(HttpContext context){
            try
            {
                //si todo esta correcto pasa al siguiente prosedimiento y que lleve todo el contexto
                await Next(context);
            }
            catch (Exception ex)
            {
                await ManejadorExcepcionAsincrono(context,ex, Logger);
            }
        }
        private async Task ManejadorExcepcionAsincrono(HttpContext context,Exception ex, ILogger<ManejadorErrorMiddleware> logger){
            object errores = null;
            switch (ex)
            {
                ///si es un arror de http
                case ManejadorExcepcion me:
                logger.LogError(ex,"Manejador Error");
                errores=me.Errores;
                context.Response.StatusCode =(int)me.Codigo;
                break;
                    //si es arror generico de c# etc
                case Exception e:
                logger.LogError(ex,"Error de servidor");
                errores= string.IsNullOrWhiteSpace(e.Message) ?"Error ": e.Message;
                context.Response.StatusCode =(int)HttpStatusCode.InternalServerError;
                break;
            }
            context.Response.ContentType="application/json";
            if(errores!= null){
                var resultados = JsonConvert.SerializeObject(new{errores});
                await context.Response.WriteAsync(resultados);
            }
            else{

            }
        
        }
    }
}