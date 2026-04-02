namespace KnowSet.Web.Configuration;

public sealed class KnowledgeBaseOptions
{
    public const string SectionName = "KnowledgeBase";

    public string LocalDocumentsPath { get; init; } = "Data/local-documents";
}
