# Unity Dev Chatbot (Low-Resource, Console, Windows-Friendly)

A lightweight C# chatbot focused on Unity development help (2D + 3D).

It uses **two answer paths**:
1. **Local Unity docs snippets** (fast, offline, very low RAM/CPU).
2. **OpenAI fallback** (only when local docs are not enough, in hybrid mode).

## Why this design

- Works on low-end machines because local retrieval is plain text/token matching (no embeddings, no vector DB, no GPU).
- Can run docs-only with no internet.
- Can scale later (better ranking, larger knowledge base, GUI, installer).

## Quick start

```bash
dotnet build
dotnet run
```

Inside chat:
- Ask Unity questions directly.
- `/mode docs` → local docs only.
- `/mode hybrid` → local docs + OpenAI fallback.
- `/help` → show help.
- `exit` → quit.

## OpenAI fallback setup (optional)

Set environment variables:

### Windows PowerShell
```powershell
$env:OPENAI_API_KEY="your_key_here"
$env:OPENAI_MODEL="gpt-4o-mini"
```

### Windows CMD
```cmd
set OPENAI_API_KEY=your_key_here
set OPENAI_MODEL=gpt-4o-mini
```

Then run `dotnet run` and keep mode as `hybrid`.

## Knowledge base

The local knowledge file is:

- `data/unity_docs_seed.json`

You can extend it with more entries from Unity docs:

```json
{
  "title": "API topic",
  "url": "Unity docs URL",
  "content": "Short practical explanation",
  "keywords": ["keyword1", "keyword2"]
}
```

Optional custom path:

```bash
UNITY_KB_PATH=path/to/your_unity_docs.json dotnet run
```

## Architecture

- `Program.cs` → console interface and command handling.
- `src/ChatbotEngine.cs` → routing logic (local docs first, OpenAI fallback second).
- `src/LocalDocSearcher.cs` → lightweight token-overlap ranking.
- `src/OpenAiClient.cs` → direct OpenAI Chat Completions API call.
- `src/Models.cs` → shared records and settings.

## Delivery plan for your open-source launch

1. **MVP now** (this repo): console assistant with docs + fallback.
2. Add a **Unity-specific KB pack** (common APIs + troubleshooting).
3. Add **telemetry-free logs** for debugging on low-end hardware.
4. Add a **small desktop UI** (WinForms/WPF/Avalonia) when stable.
5. Add **GitHub Actions CI** (build + test).
6. Publish a **single-file Windows executable** for easy sharing.

## Performance notes for tiny hardware

- Use `docs` mode by default.
- Keep KB entries concise.
- Avoid heavy dependencies.
- Prefer `dotnet publish -c Release` for faster startup.

## License

MIT (keep open source and community-driven).
