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
            Console.WriteLine("Working...");
            foreach (var Record in Types.IRecords)
            {
                Record.Value.FetchData(domain);
            }
            var lookup = new LookupClient();
            return lookup.Query(domain, QueryType.ANY); // This wont work, but needs to return something
        }
    }
}