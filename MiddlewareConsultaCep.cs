using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;

namespace AULA13ROTEAMENTOURLS
{
    public class MiddlewareConsultaCep
    {
        private readonly RequestDelegate next;

        public MiddlewareConsultaCep(RequestDelegate nextMiddleware)
        {
            next = nextMiddleware;
        }

        public async Task Invoke(HttpContext context)
        {
            string[] segmentos = context.Request.Path.ToString().Split("/", System.StringSplitOptions.RemoveEmptyEntries);
            if(segmentos.Length == 2 && segmentos[0] == "cep")
            {
               var cep = segmentos[1];
               var textoCep = await ConsultaCep(cep);
               context.Response.ContentType = "text/plain; charset=utf-8";
               await context.Response.WriteAsync(textoCep);
            }
      
            else if (next != null)
            {
                await next(context);
            }
        }
    
        private async Task<string> ConsultaCep(string cep)
        {
            var url = $"https://viacep.com.br/ws/{cep}/json";
            var cliente = new HttpClient();
            cliente.DefaultRequestHeaders.Add("User-Agent", "Middleware Consulta CEP");
            var response = await cliente.GetAsync(url);
            return await response.Content.ReadAsStringAsync();
        }
    }
}