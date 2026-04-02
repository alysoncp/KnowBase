namespace KnowSet.Web.Retrieval;

public interface IRetrievalService
{
    Task<IReadOnlyList<RetrievalHit>> SearchAsync(
        string question,
        int maxResults,
        CancellationToken cancellationToken);
}
