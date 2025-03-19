using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace SimpleFunctionCallingAgentSK;

public class ConsoleWorker : BackgroundService
{
    private readonly Kernel _kernel;
    private readonly IChatCompletionService _chatCompletionService;
    private readonly string _modelName;

    public ConsoleWorker(Kernel kernel, IChatCompletionService chatCompletionService, IConfiguration configuration)
    {
        _kernel = kernel;
        _chatCompletionService = chatCompletionService;
        _modelName = configuration["Model"]!;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            //await Task.Delay(TimeSpan.FromMilliseconds(500));
            await DoLoopAsync(stoppingToken);
        }
        catch (TaskCanceledException)
        {
            // This is fine.
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("An exception occured: {0}", ex);
        }
    }

    private async Task DoLoopAsync(CancellationToken stoppingToken)
    {
        var chatHistory = new ChatHistory();
        chatHistory.AddSystemMessage("Du bist ein freundlicher, fröhlicher Chat-Assistent. Beantworte nur Fragen, die sich auf deine Tools, Functions, Plugins beziehen!");
        chatHistory.AddSystemMessage($"Heute ist {DateTime.Now:D}.");

        while (!stoppingToken.IsCancellationRequested)
        {
            Console.Write("Your input: ");
            var input = Console.ReadLine();

            if (String.IsNullOrEmpty(input)) continue;

            chatHistory.AddUserMessage(input);

            try
            {
                var response = await _chatCompletionService.GetChatMessageContentAsync(
                    chatHistory,
                    new OpenAIPromptExecutionSettings
                    {
                        ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions,
                        Temperature = 0
                    },
                    _kernel,
                    stoppingToken);

                chatHistory.AddAssistantMessage(response.Content ?? string.Empty);
                
                Console.WriteLine($"AI output: {response.Content}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex}");
            }
        }
    }
}
