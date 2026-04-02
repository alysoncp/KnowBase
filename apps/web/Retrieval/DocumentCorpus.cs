using KnowSet.Web.Connectors;

namespace KnowSet.Web.Retrieval;

public sealed class DocumentCorpus
{
    public IReadOnlyList<RetrievalDocument> Documents { get; }

    public DocumentCorpus(IDocumentConnector connector)
    {
        var normalizedDocuments = connector.LoadDocumentsAsync(CancellationToken.None)
            .GetAwaiter()
            .GetResult();

        Documents = normalizedDocuments
            .Select(MapToRetrievalDocument)
            .ToArray();
    }

    private static RetrievalDocument MapToRetrievalDocument(Contracts.NormalizedDocument document)
    {
        var chunks = document.Chunks
            .Where(chunk => !string.IsNullOrWhiteSpace(chunk.Text))
            .ToArray();

        if (chunks.Length == 0)
        {
            throw new InvalidOperationException(
                $"Normalized document '{document.SourceDocumentId}' did not contain any text chunks.");
        }

        var summary = chunks[0].Text.Trim();
        var excerpt = (chunks.Length > 1 ? chunks[1].Text : chunks[0].Text).Trim();

        return new RetrievalDocument(
            document.SourceDocumentId,
            document.Title,
            document.CanonicalUri,
            document.DocumentType,
            document.Metadata.Client ?? "Unknown Client",
            document.Metadata.ProjectCode ?? "Unknown Project",
            summary,
            excerpt,
            ExtractKeywords(document, chunks));
    }

    private static IReadOnlyList<string> ExtractKeywords(
        Contracts.NormalizedDocument document,
        IReadOnlyList<Contracts.NormalizedDocumentChunk> chunks)
    {
        var seededKeywords = new[]
        {
            document.DocumentType,
            document.Metadata.Client,
            document.Metadata.Discipline,
            document.Metadata.ProjectCode
        }
        .Where(value => !string.IsNullOrWhiteSpace(value))
        .Select(value => value!.Trim());

        var chunkKeywords = chunks
            .SelectMany(chunk => chunk.Text.Split([' ', ',', '.', ';', ':', '(', ')', '/', '-'], StringSplitOptions.RemoveEmptyEntries))
            .Where(term => term.Length >= 5)
            .Select(term => term.ToLowerInvariant())
            .Distinct()
            .Take(8);

        return seededKeywords
            .Concat(chunkKeywords)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();
    }
}
