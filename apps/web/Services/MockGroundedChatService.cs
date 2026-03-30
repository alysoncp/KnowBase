using System.Text;
using KnowBase.Web.Contracts;
using KnowBase.Web.Retrieval;

namespace KnowBase.Web.Services;

public sealed class MockGroundedChatService(IRetrievalService retrievalService) : IChatService
{
    public async Task<ChatResponse> AskAsync(
        ChatRequest request,
        CancellationToken cancellationToken)
    {
        var normalizedQuestion = request.Question.Trim();
        var hits = await retrievalService.SearchAsync(normalizedQuestion, maxResults: 3, cancellationToken);

        if (hits.Count == 0)
        {
            return new ChatResponse(
                normalizedQuestion,
                "I could not find grounded internal references for that question in the current sample corpus. Try adding a client name, project type, discipline, or a more specific problem statement.",
                Grounded: false,
                GeneratedAtUtc: DateTimeOffset.UtcNow,
                Citations: []);
        }

        var answer = BuildAnswer(hits);
        var citations = hits
            .Select(hit => new ChatCitation(
                hit.Document.Title,
                hit.Document.CanonicalUri,
                hit.Document.Excerpt,
                hit.Document.DocumentType,
                hit.Score))
            .ToArray();

        return new ChatResponse(
            normalizedQuestion,
            answer,
            Grounded: true,
            GeneratedAtUtc: DateTimeOffset.UtcNow,
            Citations: citations);
    }

    private static string BuildAnswer(IReadOnlyList<RetrievalHit> hits)
    {
        var builder = new StringBuilder();
        var strongestHit = hits[0];

        builder.Append("The strongest internal reference is ");
        builder.Append(strongestHit.Document.Title);
        builder.Append(" (");
        builder.Append(strongestHit.Document.ProjectCode);
        builder.Append(") for ");
        builder.Append(strongestHit.Document.Client);
        builder.Append(". ");
        builder.Append(strongestHit.Document.Summary);

        if (hits.Count > 1)
        {
            builder.Append(" Related references include ");
            builder.Append(string.Join(
                "; ",
                hits.Skip(1).Select(hit => $"{hit.Document.Title} ({hit.Document.DocumentType.ToLowerInvariant()})")));
            builder.Append(". ");
        }

        builder.Append("Review the cited documents before reusing any technical detail, sequencing assumption, or client-specific requirement.");

        return builder.ToString();
    }
}
