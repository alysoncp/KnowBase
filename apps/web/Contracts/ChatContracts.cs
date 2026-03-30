namespace KnowBase.Web.Contracts;

public sealed record ChatRequest(string Question);

public sealed record ChatResponse(
    string Question,
    string Answer,
    bool Grounded,
    DateTimeOffset GeneratedAtUtc,
    IReadOnlyList<ChatCitation> Citations);

public sealed record ChatCitation(
    string Title,
    string CanonicalUri,
    string Excerpt,
    string DocumentType,
    double Score);
