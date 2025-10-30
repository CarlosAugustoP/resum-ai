using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using OpenAI.Chat;
using Resumai.Abstractions;
using Resumai.DTOs;
using Resumai.Models;

namespace Resumai.Services.Application
{
    public static class OpenAiService
    {
        public static async Task<UserResumeDTO> Call(PromptBuilder p)
        {
            ChatClient client = new(model: "gpt-4o", apiKey: Environment.GetEnvironmentVariable("OPENAI_API_KEY"));
            var (prompt, options) = p.Build();
            var msgs = new List<ChatMessage>()
            {
                new UserChatMessage(prompt)
            };
            ChatCompletion response = await client.CompleteChatAsync(messages: msgs,
            options: new ChatCompletionOptions
            {
                ResponseFormat = options
            });
            var json = response.Content[0].Text;
            return JsonSerializer.Deserialize<UserResumeDTO>(json)!;
        }
    }
}