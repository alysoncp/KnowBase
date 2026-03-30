using System.Text.RegularExpressions;

namespace KnowBase.Web.Retrieval;

public sealed partial class InMemoryRetrievalService : IRetrievalService
{
    private static readonly HashSet<string> StopWords = new(StringComparer.OrdinalIgnoreCase)
    {
        "about", "after", "been", "could", "done", "from", "have", "into", "need",
        "that", "them", "they", "this", "what", "when", "where", "which", "with",
        "would", "your", "project", "issues", "issue"
    };

    public Task<IReadOnlyList<RetrievalHit>> SearchAsync(
        string question,
        int maxResults,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var normalizedTerms = Tokenize(question);

        var results = SampleKnowledgeBase.Documents
            .Select(document => ScoreDocument(document, normalizedTerms))
            .Where(hit => hit is not null)
            .Select(hit => hit!)
            .OrderByDescending(hit => hit.Score)
            .ThenBy(hit => hit.Document.Title)
            .Take(maxResults)
            .ToArray();

        return Task.FromResult<IReadOnlyList<RetrievalHit>>(results);
    }

    private static RetrievalHit? ScoreDocument(
        RetrievalDocument document,
        IReadOnlySet<string> normalizedTerms)
    {
        if (normalizedTerms.Count == 0)
        {
            return null;
        }

        var titleTerms = Tokenize(document.Title);
        var priorityTerms = Tokenize($"{document.Client} {document.ProjectCode}");
        var bodyTerms = Tokenize($"{document.Summary} {document.Excerpt} {string.Join(' ', document.Keywords)}");

        var matchingTerms = normalizedTerms
            .Where(term => titleTerms.Contains(term) || priorityTerms.Contains(term) || bodyTerms.Contains(term))
            .OrderBy(term => term)
            .ToArray();

        if (matchingTerms.Length == 0)
        {
            return null;
        }

        // Keep the heuristic simple for v1: exact title/project/client matches are weighted above body-only matches.
        var score = matchingTerms.Sum(term =>
        {
            if (titleTerms.Contains(term))
            {
                return 2.2;
            }

            if (priorityTerms.Contains(term))
            {
                return 1.8;
            }

            return 1.0;
        });

        return new RetrievalHit(document, Math.Round(score, 2), matchingTerms);
    }

    private static IReadOnlySet<string> Tokenize(string value)
    {
        return TermPattern()
            .Matches(value.ToLowerInvariant())
            .Select(match => match.Value)
            .Where(term => term.Length >= 3 && !StopWords.Contains(term))
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
    }

    [GeneratedRegex("[a-z0-9]+")]
    private static partial Regex TermPattern();
}
