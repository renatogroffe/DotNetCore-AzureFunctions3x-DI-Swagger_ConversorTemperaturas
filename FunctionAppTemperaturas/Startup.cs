using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using FunctionAppTemperaturas.Configurations;

[assembly: FunctionsStartup(typeof(FunctionAppTemperaturas.Startup))]
namespace FunctionAppTemperaturas
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            // Projeto que serviu de base para esta implementação:
            // https://github.com/Azure/azure-functions-openapi-extension/tree/main/samples/Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.V3IoC
            builder.Services.AddSingleton<AppSettings>();
        }
    }
}