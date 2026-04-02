using System.Text.Json;
using Microsoft.Extensions.Options;
using KnowSet.Web.Configuration;
using KnowSet.Web.Contracts;

namespace KnowSet.Web.Connectors;

public sealed class LocalFolderDocumentConnector(
    IOptions<KnowledgeBaseOptions> options,
    IHostEnvironment hostEnvironment) : IDocumentConnector
{
    public Task<IReadOnlyList<NormalizedDocument>> LoadDocumentsAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var relativePath = options.Value.LocalDocumentsPath;
        var absolutePath = Path.Combine(hostEnvironment.ContentRootPath, relativePath);

        if (!Directory.Exists(absolutePath))
        {
            throw new InvalidOperationException(
                $"Local documents folder was not found at '{absolutePath}'.");
        }

        var documents = Directory
            .EnumerateFiles(absolutePath, "*.json", SearchOption.AllDirectories)
            .OrderBy(path => path, StringComparer.OrdinalIgnoreCase)
            .Select(path => LoadDocument(path))
            .ToArray();

        if (documents.Length == 0)
        {
            throw new InvalidOperationException(
                $"Local documents folder '{absolutePath}' did not contain any normalized document files.");
        }

        return Task.FromResult<IReadOnlyList<NormalizedDocument>>(documents);
    }

    private static NormalizedDocument LoadDocument(string path)
    {
        var json = File.ReadAllText(path);
        var document = JsonSerializer.Deserialize<NormalizedDocument>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (document is null)
        {
            throw new InvalidOperationException(
                $"Document file '{path}' could not be deserialized into a normalized document.");
        }

        return document;
    }
}
