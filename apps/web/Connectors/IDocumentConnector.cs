using KnowSet.Web.Contracts;

namespace KnowSet.Web.Connectors;

public interface IDocumentConnector
{
    Task<IReadOnlyList<NormalizedDocument>> LoadDocumentsAsync(CancellationToken cancellationToken);
}
