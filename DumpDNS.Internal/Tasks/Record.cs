using System;
using DnsClient;
using DumpDNS.Internal.Records;

namespace DumpDNS.Internal.Tasks;

public class Record<T> : ITask
{
    public Record(Types.LookupInfo lookup, LookupClient client, ref IRecord<T> record)
    {
        TaskName = $"Fetching {typeof(T)} records";
        Lookup = lookup;
        Client = client;
        IRecord = record;
    }

    public string TaskName { get; }

    public Types.LookupInfo Lookup;
    public LookupClient Client;
    public IRecord<T> IRecord;

    public Action<OngoingTask> Action { get; } = async task =>
    {
        if (!task.ITask.GetType().IsAssignableTo(typeof(Record<T>)))
            throw new ArgumentException($"{task} is not assignable to {typeof(Record<T>)}");

        var record = task.ITask as Record<T>;
        await record!.IRecord.FetchDataAsync(record!.Client, record!.Lookup);
    };
}
