using KnowBase.Web.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<PlatformOptions>(
    builder.Configuration.GetSection(PlatformOptions.SectionName));

builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler();

app.MapGet("/", () => Results.Ok(new
{
    service = "KnowBase.Web",
    status = "ok",
    message = "KnowBase web application scaffold is running."
}));

app.MapGet("/health", () => Results.Ok(new
{
    status = "healthy",
    utc = DateTimeOffset.UtcNow
}));

app.Run();
