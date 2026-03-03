using DnsClient;

namespace DumpDNS.Functionality
{
    public static class Dump
    {
        public static IDnsQueryResponse Start(string domain, (int, int) dimensions)
        {
            Program.ActiveInstructions = Program.BottomInstructions.ProcessingNoCancel;
            if(Program.Render != null) Program.Render(null, dimensions);
            Console.CursorTop = 2;
            Console.CursorLeft = 0;
            Console.Write("Working...");
            foreach (var Record in Types.IRecords)
            {
                Record.Value.FetchData(domain);
                Console.Write($"\rWorking... {Record.Key.ToString().PadRight(8)}");
            }
            var lookup = new LookupClient();
            return lookup.Query(domain, QueryType.ANY); // This wont work, but needs to return something
        }
    }
}