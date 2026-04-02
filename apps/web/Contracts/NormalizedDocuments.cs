namespace KnowSet.Web.Contracts;

public sealed record NormalizedDocument(
    string SourceSystem,
    string SourceDocumentId,
    string CanonicalUri,
    string Title,
    string DocumentType,
    DateTimeOffset LastModifiedUtc,
    IReadOnlyList<string> AclPrincipals,
    NormalizedDocumentMetadata Metadata,
    IReadOnlyList<NormalizedDocumentChunk> Chunks);

public sealed record NormalizedDocumentMetadata(
    string? Client,
    string? Discipline,
    string? ProjectCode);

public sealed record NormalizedDocumentChunk(
    string ChunkId,
    string Text);
