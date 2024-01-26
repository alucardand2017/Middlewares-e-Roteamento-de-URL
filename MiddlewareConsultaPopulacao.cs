using System;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;

namespace AULA13ROTEAMENTOURLS
{
    public class MiddlewareConsultaPopulacao
    {
        private readonly RequestDelegate next;
        public MiddlewareConsultaPopulacao()
        {
            
        }
        public MiddlewareConsultaPopulacao(RequestDelegate nextMiddleware)
        {
            next = nextMiddleware;
        }

        public async Task Invoke(HttpContext context)
        {
            string localidade = HttpUtility.UrlDecode(context.Request.RouteValues["local"] as string);
           
            var populacao = (new Random().Next(999, 999999));
            context.Response.ContentType = "text/html; charset=utf-8";
            StringBuilder html = new StringBuilder();
            html.Append($"<h3>População de {localidade.ToUpper()}</h3>");
            html.Append($"<p>{populacao:N0} habitantes</p>");      
            await context.Response.WriteAsync(html.ToString());   
          
            if (next != null)
            {
                await next(context);
            }
        }
    }
}