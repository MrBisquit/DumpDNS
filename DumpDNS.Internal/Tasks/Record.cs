using System;
using DnsClient;
using DnsClient.Protocol;
using DumpDNS.Internal.Records;

namespace DumpDNS.Internal.Tasks;

public class Record
{
    
}

public class Record<T> : Record, ITask
{
    public Record(Types.LookupInfo lookup, LookupClient client, Types.DnsRecordType type)
    {
        TaskName = $"Fetching {typeof(T)} records";
        Lookup = lookup;
        Client = client;
        var result = IRecord.Create(type);
        if(result == null)
        {
            throw new Exception($"IRecord.Create({type}) returned {result}");
        }
        IRecord = result;
    }

    public Record(Types.LookupInfo lookup, LookupClient client, ref IRecord<T> record)
    {
        TaskName = $"Fetching {typeof(T)} records";
        Lookup = lookup;
        Client = client;
        IRecord = record;
    }

    public string TaskName { get; }

    public Guid TaskID { get; } = Guid.NewGuid();

    public HashSet<Guid> WaitingFor { get; set; } = [];

    public Types.LookupInfo Lookup;
    public LookupClient Client;
    public IRecord IRecord;

    public Action<OngoingTask> Action { get; } = async task =>
    {
        if (!task.ITask.GetType().IsAssignableTo(typeof(Record<T>)))
            throw new ArgumentException($"{task} is not assignable to {typeof(Record<T>)}");

        var record = task.ITask as Record<T>;
        await record!.IRecord.FetchDataAsync(record!.Client, record!.Lookup);
    };
}
