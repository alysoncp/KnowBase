namespace KnowBase.Web.Retrieval;

public sealed record RetrievalDocument(
    string DocumentId,
    string Title,
    string CanonicalUri,
    string DocumentType,
    string Client,
    string ProjectCode,
    string Summary,
    string Excerpt,
    IReadOnlyList<string> Keywords);

public sealed record RetrievalHit(
    RetrievalDocument Document,
    double Score,
    IReadOnlyList<string> MatchingTerms);
