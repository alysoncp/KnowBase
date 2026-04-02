using KnowSet.Web.Contracts;

namespace KnowSet.Web.Retrieval;

public interface IKnowledgeCatalogService
{
    Task<IReadOnlyList<DocumentCatalogItem>> GetDocumentsAsync(CancellationToken cancellationToken);
}
