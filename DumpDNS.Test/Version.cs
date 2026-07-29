using System;
using System.Runtime.InteropServices;
using DumpDNS.Internal;

namespace DumpDNS.Test;

public class Version
{
    [Fact]
    public void Check_Higher()
    {
        Assert.True(Internal.Utils.IsHigher("1.0.0", "1.0.1"));
        Assert.True(Internal.Utils.IsHigher("1.0.0", "1.0.10"));
        Assert.True(Internal.Utils.IsHigher("1.0.0", "1.10.0"));
        Assert.True(Internal.Utils.IsHigher("1.0.0", "2.10.10"));
        Assert.True(Internal.Utils.IsHigher("1.0.0", "2.0.0"));

        Assert.True(Internal.Utils.IsHigher("1.0.0", "v2.0.0"));
        Assert.True(Internal.Utils.IsHigher("v1.0.0", "2.0.0"));
        Assert.True(Internal.Utils.IsHigher("v1.0.0", "v2.0.0"));
    }

    [Fact]
    public void Check_Lower()
    {
        Assert.False(Internal.Utils.IsHigher("1.0.1", "1.0.0"));
        Assert.False(Internal.Utils.IsHigher("1.0.10", "1.0.0s"));
        Assert.False(Internal.Utils.IsHigher("1.10.0", "1.0.0"));
        Assert.False(Internal.Utils.IsHigher("2.10.10", "1.0.0"));
        Assert.False(Internal.Utils.IsHigher("2.0.0", "1.0.0"));

        Assert.False(Internal.Utils.IsHigher("2.0.0", "v1.0.0"));
        Assert.False(Internal.Utils.IsHigher("v2.0.0", "1.0.0"));
        Assert.False(Internal.Utils.IsHigher("v2.0.0", "v1.0.0"));
    }

    [Fact]
    public void Check_Equals()
    {
        
    }

    /*[Fact]
    public async void Check_Actual()
    {
        string[] testValues =
        [
            //"0.0.0",
            "1.0.0",
            "1.0.1",
            "1.0.10",
            //"1.0.0.0"
        ];

        Internal.Tasks.Version v = new();
        Internal.ITask.Enqueue(v);
        await ITask.StartQueue();

        foreach (var test in testValues)
        {
            Console.WriteLine(Global.Version);
            Assert.True(Internal.Utils.IsHigher(test, Global.Version));
        }
    }*/
}