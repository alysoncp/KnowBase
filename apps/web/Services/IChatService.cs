using KnowBase.Web.Contracts;

namespace KnowBase.Web.Services;

public interface IChatService
{
    Task<ChatResponse> AskAsync(ChatRequest request, CancellationToken cancellationToken);
}
