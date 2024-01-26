
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
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
                endpoints.MapGet("pop/{local}", EndpointConsultaPop.Endpoint)
                    .WithMetadata(new RouteNameMetadata("consultapop"));
                endpoints.MapGet("cep/{cep}", EndpointConsultaCep.Endpoint);
            });
            app.Use(async (context, next) => {
                context.Response.ContentType = "text/plain; charset=utf-8";
                await context.Response.WriteAsync("Middleware terminal alcançado. ");
            });
    
        }
    };
}

