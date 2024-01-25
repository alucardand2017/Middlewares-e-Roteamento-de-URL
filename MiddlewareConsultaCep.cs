using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace AULA13ROTEAMENTOURLS
{
    public class MiddlewareConsultaCep
    {
        private readonly RequestDelegate next;

        public MiddlewareConsultaCep(RequestDelegate nextMiddleware)
        {
            next = nextMiddleware;
        }
        public MiddlewareConsultaCep()
        {
            
        }

        public async Task Invoke(HttpContext context)
        {
            string[] segmentos = context.Request.Path.ToString().
            Split("/", System.StringSplitOptions.RemoveEmptyEntries);
            if(segmentos.Length == 2 && segmentos[0] == "cep")
            {
                var cep = segmentos[1];
                var objetoCEP = await ConsultaCep(cep);
                context.Response.ContentType = "text/html; charset=utf-8";

                StringBuilder html = new StringBuilder();

                html.Append($"<h3>Dados de CEP {objetoCEP.cep}</h3>");
                html.Append($"<p>Logradouro :{objetoCEP.logradouro}</p>");
                html.Append($"<p>Bairro: {objetoCEP.bairro}</p>");
                html.Append($"<p>Cidade/UF {objetoCEP.localidade}/{objetoCEP.uf}</p>");

                string localidade = HttpUtility.UrlEncode($"{objetoCEP.localidade}-{objetoCEP.uf}");
                html.Append($"<p><a href='/pop/{localidade}'> Consultar População</a></p>");

                await context.Response.WriteAsync(html.ToString());
            }
      
            else if (next != null)
            {
                await next(context);
            }
        }
    
        private async Task<JsonCep> ConsultaCep(string cep)
        {
            var url = $"https://viacep.com.br/ws/{cep}/json";
            var cliente = new HttpClient();
            cliente.DefaultRequestHeaders.Add("User-Agent", "Middleware Consulta CEP");
            var response = await cliente.GetAsync(url);

            var dadosCEP = await response.Content.ReadAsStringAsync();
            dadosCEP = dadosCEP.Replace("?(", "").Replace(");", "").Trim();
            return JsonConvert.DeserializeObject<JsonCep>(dadosCEP);

        }
    }
}