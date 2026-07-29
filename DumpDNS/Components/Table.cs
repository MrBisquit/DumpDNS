using System;

namespace DumpDNS.Components;

public class Table
{
    public string? Title { get; set; }
    public List<string> Headers { get; set; } = [];
    public List<List<string>> Rows { get; set; } = [];
}