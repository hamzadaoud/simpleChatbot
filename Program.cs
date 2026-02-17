using SimpleChatbot;

Console.WriteLine("Unity Dev Chatbot (Low-Resource Edition)");
Console.WriteLine("Type your question, or type 'exit' to quit.");
Console.WriteLine("Commands: /mode docs | /mode hybrid | /help");

var options = AppOptions.LoadFromEnvironment();
var engine = await ChatbotEngine.CreateAsync(options);

while (true)
{
    Console.Write("\nYou> ");
    var input = Console.ReadLine()?.Trim();

    if (string.IsNullOrWhiteSpace(input))
    {
        continue;
    }

    if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
    {
        break;
    }

    if (input.StartsWith("/mode", StringComparison.OrdinalIgnoreCase))
    {
        var mode = input.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Skip(1).FirstOrDefault();
        if (mode is "docs" or "hybrid")
        {
            options = options with { Mode = mode };
            engine = await ChatbotEngine.CreateAsync(options);
            Console.WriteLine($"Bot> Mode set to '{mode}'.");
        }
        else
        {
            Console.WriteLine("Bot> Invalid mode. Use '/mode docs' or '/mode hybrid'.");
        }

        continue;
    }

    if (input.Equals("/help", StringComparison.OrdinalIgnoreCase))
    {
        Console.WriteLine("Bot> Ask Unity questions. In docs mode it only uses local Unity docs snippets. In hybrid mode it falls back to OpenAI if no local answer is strong enough.");
        Console.WriteLine("Bot> Set OPENAI_API_KEY and optional OPENAI_MODEL (default: gpt-4o-mini) for fallback.");
        continue;
    }

    var response = await engine.AnswerAsync(input);
    Console.WriteLine($"Bot> {response.Text}");

    if (!string.IsNullOrWhiteSpace(response.Source))
    {
        Console.WriteLine($"Source: {response.Source}");
    }
}
