using System;
using System.Threading.Tasks;
using Microsoft.SemanticKernel;

namespace EduCorePro.Services;

public class AiService
{
    private readonly SettingsService _settings;

    public AiService(SettingsService settings)
    {
        _settings = settings;
    }

    public async Task<string> SendPromptAsync(string prompt)
    {
        var apiKey = _settings.GetApiKey();
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            return "Ошибка: API ключ не настроен. Пожалуйста, зайдите в Настройки (шестеренка).";
        }

        try
        {
            var builder = Kernel.CreateBuilder();
            builder.AddOpenAIChatCompletion(
                modelId: "meta-llama/llama-4-scout-17b-16e-instruct",
                apiKey: apiKey,
                endpoint: new Uri("https://api.groq.com/openai/v1")
            );

            var kernel = builder.Build();
            
            var response = await kernel.InvokePromptAsync(prompt);
            return response.ToString();
        }
        catch (Exception ex)
        {
            return $"Произошла ошибка при обращении к ИИ:\n{ex.Message}";
        }
    }
}