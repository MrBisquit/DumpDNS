namespace DumpDNS.Internal;

public static class Global
{
    public static readonly string Author = "MrBisquit";
    public static readonly string Repo = "DumpDNS";
    public static readonly string RepoBase = $"https://github.com/{Author}/{Repo}";
    public static readonly string RangesSourcePath = Repo + "/raw/refs/heads/master/.sources";

    // Version information
    public static string Version = "0.0.0";
    public static bool VersionAvailable = false;
    public static string VersionString = "";
    public static bool VersionUpdateAvailable = false;
    public static bool VersionUnreleased = false;

    // Other information
    public static readonly int ConcurrentTasks = 5;
    public static readonly int MaxGridRowHeight = 5;
}
