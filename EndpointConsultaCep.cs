using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace AULA13ROTEAMENTOURLS
{
    public static class EndpointConsultaCep
    {


        public static async Task Endpoint(HttpContext context)
        {            
            string cep = context.Request.RouteValues["cep"] as string;
            var objetoCEP = await ConsultaCep(cep);
            if(objetoCEP == null)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;

            }
            else
            {
                context.Response.ContentType = "text/html; charset=utf-8";
                StringBuilder html = new StringBuilder();
                html.Append($"<h3>Dados de CEP {objetoCEP.cep}</h3>");
                html.Append($"<p>Logradouro :{objetoCEP.logradouro}</p>");
                html.Append($"<p>Bairro: {objetoCEP.bairro}</p>");
                html.Append($"<p>Cidade/UF {objetoCEP.localidade}/{objetoCEP.uf}</p>");
                string localidade = HttpUtility.UrlEncode($"{objetoCEP.localidade}-{objetoCEP.uf}");
                
                LinkGenerator geradorLink = context.RequestServices.GetService<LinkGenerator>();
                string url = geradorLink.GetPathByRouteValues(
                context, "consultapop", new {local = localidade});            
            
                html.Append($"<p><a href='{url}'> Consultar População</a></p>");
                await context.Response.WriteAsync(html.ToString());  
            }
        }
    
        private static async Task<JsonCep> ConsultaCep(string cep)
        {
            var url = $"https://viacep.com.br/ws/{cep}/json";
            var cliente = new HttpClient();
            cliente.DefaultRequestHeaders.Add("User-Agent", "Middleware Consulta CEP");
            var response = await cliente.GetAsync(url);

            var dadosCEP = await response.Content.ReadAsStringAsync();
            dadosCEP = dadosCEP.Replace("?(", "").Replace(");", "").Trim();
            return dadosCEP.Contains("\"erro\":") ? null : JsonConvert.DeserializeObject<JsonCep>(dadosCEP);

        }
    }
}