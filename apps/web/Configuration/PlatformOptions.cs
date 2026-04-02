namespace KnowSet.Web.Configuration;

public sealed class PlatformOptions
{
    public const string SectionName = "Platform";

    public string EnvironmentName { get; init; } = "Unknown";

    public bool RequireWindowsAuthentication { get; init; }
}
