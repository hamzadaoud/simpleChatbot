namespace SimpleChatbot;

public sealed class ChatbotEngine
{
    private readonly AppOptions _options;
    private readonly LocalDocSearcher _searcher;
    private readonly OpenAiClient? _openAi;

    private ChatbotEngine(AppOptions options, LocalDocSearcher searcher, OpenAiClient? openAi)
    {
        _options = options;
        _searcher = searcher;
        _openAi = openAi;
    }

    public static async Task<ChatbotEngine> CreateAsync(AppOptions options)
    {
        var searcher = await LocalDocSearcher.LoadAsync(options.KnowledgeBasePath);
        OpenAiClient? openAi = null;

        if (!string.IsNullOrWhiteSpace(options.OpenAiApiKey) && options.Mode == "hybrid")
        {
            openAi = new OpenAiClient(new HttpClient(), options.OpenAiApiKey!, options.OpenAiModel);
        }

        return new ChatbotEngine(options, searcher, openAi);
    }

    public async Task<ChatResponse> AnswerAsync(string question)
    {
        var (entry, score) = _searcher.Search(question);

        if (entry is not null && score >= 0.14)
        {
            return new ChatResponse($"{entry.Title}: {entry.Content}", entry.Url);
        }

        if (_options.Mode == "docs")
        {
            return new ChatResponse("I couldn't find a strong match in local Unity docs snippets. Try rephrasing with API names (e.g., Rigidbody, Input, NavMesh).", "Local docs only");
        }

        if (_openAi is null)
        {
            return new ChatResponse("No OpenAI fallback configured. Set OPENAI_API_KEY or switch to /mode docs.");
        }

        var llmAnswer = await _openAi.AskAsync(question);
        if (string.IsNullOrWhiteSpace(llmAnswer))
        {
            return new ChatResponse("OpenAI fallback failed right now. Try again later or use /mode docs.");
        }

        return new ChatResponse(llmAnswer, "OpenAI fallback");
    }
}
