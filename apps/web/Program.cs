using KnowBase.Web.Configuration;
using KnowBase.Web.Contracts;
using KnowBase.Web.Retrieval;
using KnowBase.Web.Services;
using KnowBase.Web.Ui;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<PlatformOptions>(
    builder.Configuration.GetSection(PlatformOptions.SectionName));

builder.Services.AddProblemDetails();
builder.Services.AddSingleton<IRetrievalService, InMemoryRetrievalService>();
builder.Services.AddSingleton<IChatService, MockGroundedChatService>();

var app = builder.Build();

app.UseExceptionHandler();

app.MapGet("/", () => Results.Content(HomePage.Render(), "text/html"));

app.MapGet("/health", () => Results.Ok(new
{
    status = "healthy",
    utc = DateTimeOffset.UtcNow
}));

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
