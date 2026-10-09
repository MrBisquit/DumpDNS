using System;
using System.Reflection;
using DnsClient;
using DumpDNS.Internal.Records;
using Octokit;

namespace DumpDNS.Internal.Tasks;

public class Ranges : ITask
{
    public string TaskName { get; } = "Updating ranges";

    public Guid TaskID { get; } = Guid.NewGuid();

    public HashSet<Guid> WaitingFor { get; set; } = [];

    public Action<OngoingTask> Action { get; } = async task =>
    {
        Internal.Ranges.LoadRanges();
        Internal.Ranges.UpdateRanges();
    };
}
