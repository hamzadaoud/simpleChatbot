namespace SimpleChatbot;

public sealed record AppOptions(string Mode, string KnowledgeBasePath, string? OpenAiApiKey, string OpenAiModel)
{
    public static AppOptions LoadFromEnvironment()
    {
        return new AppOptions(
            Mode: "hybrid",
            KnowledgeBasePath: Environment.GetEnvironmentVariable("UNITY_KB_PATH") ?? "data/unity_docs_seed.json",
            OpenAiApiKey: Environment.GetEnvironmentVariable("OPENAI_API_KEY"),
            OpenAiModel: Environment.GetEnvironmentVariable("OPENAI_MODEL") ?? "gpt-4o-mini");
    }
}

public sealed record ChatResponse(string Text, string? Source = null);

public sealed record DocEntry(string Title, string Url, string Content, string[] Keywords);
