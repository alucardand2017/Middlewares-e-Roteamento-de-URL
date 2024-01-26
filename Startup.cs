
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
            services.Configure<RouteOptions>(opt => 
            {
                opt.ConstraintMap.Add("parametroLocal", typeof(ConstraintParametroLocal));
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            //app.UseMiddleware<MiddlewareConsultaPopulacao>();
            //app.UseMiddleware<MiddlewareConsultaCep>();



            app.UseRouting();
            app.UseEndpoints(endpoints => {

                endpoints.MapGet("arq/{arquivo}.{ext}" , async context =>
                {
                    context.Response.ContentType = "text/plain; charset=uft-8";
                    await context.Response.WriteAsync("Requisicao foi roteada. \n");
                    foreach(var item in context.Request.RouteValues)
                    {
                        await context.Response.WriteAsync($"{item.Key}: {item.Value}\n");
                    }
                });

                endpoints.MapGet("rota", async context => {
                    context.Response.ContentType = "text/plain; charset=utf-8";
                    await context.Response.WriteAsync("Requisição roteada");
                });

                endpoints.MapGet("pop/{*local:parametroLocal}", EndpointConsultaPop.Endpoint) //o asterisco desconsidera o separador após o parâmetro na URL
                    .WithMetadata(new RouteNameMetadata("consultapop"));

                endpoints.MapGet("cep/{cep:regex(^\\d{{8}}$)?}", EndpointConsultaCep.Endpoint);
            });

            app.Use(async (context, next) => {
                context.Response.ContentType = "text/plain; charset=utf-8";
                await context.Response.WriteAsync("Middleware terminal alcançado. ");
            });
    
        }
    };
}

