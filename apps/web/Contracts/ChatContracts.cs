namespace KnowSet.Web.Contracts;

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

public sealed record DocumentCatalogItem(
    string DocumentId,
    string Title,
    string Client,
    string ProjectCode,
    string DocumentType,
    string Summary,
    IReadOnlyList<string> Keywords);
