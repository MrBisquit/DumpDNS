using System;
using System.Reflection;
using DnsClient;
using DumpDNS.Internal.Records;
using Octokit;

namespace DumpDNS.Internal.Tasks;

public class Version : ITask
{
    public string TaskName { get; } = "Fetching version information";

    public Action<OngoingTask> Action { get; } = async task =>
    {
        Global.Version = Assembly.GetExecutingAssembly().GetName().Version!.ToString();
        List<string> split = Global.Version.Split('.').ToList();
        split.RemoveAt(split.Count - 1);
        Global.Version = string.Join('.', split);

        GitHubClient client = new(new ProductHeaderValue("DumpDNS"));
        var release = await client.Repository.Release.GetLatest("MrBisquit", "DumpDNS");

        if (Utils.IsHigher(Global.Version, release.TagName))
        {
            Global.VersionString = $"{Global.Version} < {release.TagName}";
            Global.VersionUpdateAvailable = true;
        }
        else if (Utils.IsHigher(release.TagName, Global.Version))
        {
            Global.VersionString = $"{Global.Version}-unreleased";
            Global.VersionUnreleased = true;
        }
        else
        {
            Global.VersionString = Global.Version;
        }
    };
}
