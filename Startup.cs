
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace AULA13ROTEAMENTOURLS
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            //app.UseMiddleware<MiddlewareConsultaPopulacao>();
            //app.UseMiddleware<MiddlewareConsultaCep>();

            app.UseRouting();
            app.UseEndpoints(endpoints => {
                endpoints.MapGet("rota", async context => {
                    context.Response.ContentType = "text/plain; charset=utf-8";
                    await context.Response.WriteAsync("Requisição roteada");
                });
                endpoints.MapGet("pop/guarulhos-sp", new MiddlewareConsultaPopulacao().Invoke);
                endpoints.MapGet("cep/01001000", new MiddlewareConsultaCep().Invoke);
            });
            app.Use(async (context, next) => {
                context.Response.ContentType = "text/plain; charset=utf-8";
                await context.Response.WriteAsync("Middleware terminal alcançado. ");
            });
    
        }
    };
}

