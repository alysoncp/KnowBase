using KnowSet.Web.Configuration;
using KnowSet.Web.Connectors;
using KnowSet.Web.Contracts;
using KnowSet.Web.Retrieval;
using KnowSet.Web.Services;
using KnowSet.Web.Ui;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<PlatformOptions>(
    builder.Configuration.GetSection(PlatformOptions.SectionName));
builder.Services.Configure<KnowledgeBaseOptions>(
    builder.Configuration.GetSection(KnowledgeBaseOptions.SectionName));

builder.Services.AddProblemDetails();
builder.Services.AddSingleton<IDocumentConnector, LocalFolderDocumentConnector>();
builder.Services.AddSingleton<DocumentCorpus>();
builder.Services.AddSingleton<InMemoryRetrievalService>();
builder.Services.AddSingleton<IRetrievalService>(serviceProvider =>
    serviceProvider.GetRequiredService<InMemoryRetrievalService>());
builder.Services.AddSingleton<IKnowledgeCatalogService>(serviceProvider =>
    serviceProvider.GetRequiredService<InMemoryRetrievalService>());
builder.Services.AddSingleton<IChatService, MockGroundedChatService>();

var app = builder.Build();

app.UseExceptionHandler();

app.MapGet("/", () => Results.Content(HomePage.Render(), "text/html"));

app.MapGet("/health", () => Results.Ok(new
{
    status = "healthy",
    utc = DateTimeOffset.UtcNow
}));

app.MapGet("/api/documents", async (
    IKnowledgeCatalogService knowledgeCatalogService,
    CancellationToken cancellationToken) =>
{
    var documents = await knowledgeCatalogService.GetDocumentsAsync(cancellationToken);
    return Results.Ok(documents);
});

app.MapPost("/api/chat", async (
    ChatRequest request,
    IChatService chatService,
    CancellationToken cancellationToken) =>
{
    if (string.IsNullOrWhiteSpace(request.Question))
    {
        return Results.ValidationProblem(new Dictionary<string, string[]>
        {
            ["question"] = ["A project question is required."]
        });
    }

    var response = await chatService.AskAsync(request, cancellationToken);
    return Results.Ok(response);
});

app.Run();
