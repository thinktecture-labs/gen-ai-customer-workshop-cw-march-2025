using Microsoft.Extensions.Hosting;
using Serilog;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using SimpleFunctionCallingAgentSK.Plugins;
using SimpleFunctionCallingAgentSK;
using System;
using System.Net.Http;

try
{
    //var httpClient = new HttpClient(new HttpClientHandler { Proxy = new LocalDebuggingProxy() });

    var host = Host
        .CreateDefaultBuilder(args)
        .UseConsoleLifetime()
        .UseSerilog((ctx, logger) => logger.ReadFrom.Configuration(ctx.Configuration))
        .ConfigureServices((ctx, services) =>
        {
            var kernelBuilder = Kernel.CreateBuilder()
                .AddOpenAIChatCompletion(
                    ctx.Configuration["Model"] ?? "gpt-4o", 
                    ctx.Configuration["OPENAI_API_KEY"]);
                    //httpClient: httpClient);
                

            kernelBuilder.Plugins.AddFromType<EmployeePlugin>();
            kernelBuilder.Plugins.AddFromType<BookingPlugin>();
            
            var kernel = kernelBuilder.Build();
            services.AddSingleton(kernel);
            services.AddSingleton<IChatCompletionService>(kernel.GetRequiredService<IChatCompletionService>());
            
            services.AddHostedService<ConsoleWorker>();
        })       
        .Build();

    await host.RunAsync();
}
catch (Exception ex)
{
    Console.Error.WriteLine("An exception occured: {0}", ex);
    Log.Fatal(ex, "A fatal exception caused the application to crash");
}
finally
{
    await Log.CloseAndFlushAsync();
}
