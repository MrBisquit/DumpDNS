namespace DumpDNS.Internal;

public static class Global
{
    public static readonly string Author = "MrBisquit";
    public static readonly string Repo = "DumpDNS";
    public static readonly string RepoBase = $"https://github.com/{Author}/{Repo}";
    public static readonly string RangesSourcePath = Repo + "/raw/refs/heads/master/.sources";

    // Version information
    public static bool VersionAvailable = false;
    public static string VersionString = "";
    public static bool VersionUpdateAvailable = false;
    public static bool VersionUnreleased = false;
}