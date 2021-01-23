using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;

namespace ConsoleApp_Core_Logging_DI
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            BuildConfig(builder);

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Build()) //Takes configuration info from the builded configuration
                .Enrich.FromLogContext() //Enrich logs with properties inside Serilog.Context
                .WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}") //You can change theme adding: theme: AnsiConsoleTheme.Code
                .CreateLogger();

            Log.Logger.Information("Application Start");

            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddTransient<Service>();                    
                })
                .Build();

            //Create the instance through injection of host (that has everything, DI, logging, config)
            var svc = ActivatorUtilities.CreateInstance<Service>(host.Services);
            svc.MyService();
        }


        static void BuildConfig(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .AddEnvironmentVariables();

        }
    }

}
