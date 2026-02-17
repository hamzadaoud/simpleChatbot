using System.Text.Json;
using System.Text.RegularExpressions;

namespace SimpleChatbot;

public sealed class LocalDocSearcher
{
    private readonly IReadOnlyList<DocEntry> _entries;
    private static readonly Regex TokenRegex = new("[a-zA-Z0-9_]+", RegexOptions.Compiled);

    private LocalDocSearcher(IReadOnlyList<DocEntry> entries)
    {
        _entries = entries;
    }

    public static async Task<LocalDocSearcher> LoadAsync(string path)
    {
        await using var fs = File.OpenRead(path);
        var entries = await JsonSerializer.DeserializeAsync<List<DocEntry>>(fs, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? new List<DocEntry>();

        return new LocalDocSearcher(entries);
    }

    public (DocEntry? Entry, double Score) Search(string question)
    {
        var queryTokens = Tokenize(question);
        if (queryTokens.Count == 0)
        {
            return (null, 0);
        }

        DocEntry? best = null;
        var bestScore = 0.0;

        foreach (var entry in _entries)
        {
            var docText = $"{entry.Title} {entry.Content} {string.Join(' ', entry.Keywords)}";
            var docTokens = Tokenize(docText);
            var overlap = queryTokens.Intersect(docTokens).Count();

            if (overlap == 0)
            {
                continue;
            }

            var score = (double)overlap / Math.Sqrt(queryTokens.Count * docTokens.Count);
            if (score > bestScore)
            {
                bestScore = score;
                best = entry;
            }
        }

        return (best, bestScore);
    }

    private static HashSet<string> Tokenize(string text)
    {
        return TokenRegex.Matches(text.ToLowerInvariant())
            .Select(m => m.Value)
            .Where(t => t.Length > 1)
            .ToHashSet();
    }
}
