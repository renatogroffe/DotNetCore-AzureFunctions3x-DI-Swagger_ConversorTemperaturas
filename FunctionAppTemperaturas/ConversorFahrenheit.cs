using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using FunctionAppTemperaturas.Models;

namespace FunctionAppTemperaturas
{
    public class ConversorFahrenheit
    {
        [FunctionName("ConversorFahrenheit")]
        [OpenApiOperation(operationId: "ConversorTemperaturas", tags: new[] { "Temperaturas" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "temperaturaFahrenheit", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "Temperatura em graus Fahrenheit")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Temperatura), Description = "Resultado da conversão de uma temperatura em Fahrenheit")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(FalhaConversao), Description = "Falha na conversão de uma temperatura em Fahrenheit")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get",
                Route = "ConversorTemperaturas/Fahrenheit/{temperaturaFahrenheit}")] HttpRequest req,
            ILogger log, double temperaturaFahrenheit)
        {
            log.LogInformation(
                $"Recebida temperatura para conversão: {temperaturaFahrenheit}");

            if (temperaturaFahrenheit >= -459.67) // Temperatura maior ou igual ao zero absoluto
            {
                var resultado = new Temperatura(temperaturaFahrenheit);
                log.LogInformation($"{resultado.Fahrenheit} graus Fahrenheit = " +
                    $"{resultado.Celsius} graus Celsius = " +
                    $"{resultado.Kelvin} graus Kelvin");
                return new OkObjectResult(resultado);
            }
            else
            {
                log.LogError(
                    $"Informe uma temperatura válida! Valor recebido: {temperaturaFahrenheit}");
                return new BadRequestObjectResult(
                    new FalhaConversao()
                    {
                        Mensagem = $"Preencha o parâmetro '{nameof(temperaturaFahrenheit)}' com uma temperatura válida!"
                    });
            }
        }
    }
}