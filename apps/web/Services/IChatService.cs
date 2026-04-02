using KnowSet.Web.Contracts;

namespace KnowSet.Web.Services;

public interface IChatService
{
    Task<ChatResponse> AskAsync(ChatRequest request, CancellationToken cancellationToken);
}
