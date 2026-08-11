using System;
using System.Runtime.InteropServices;
using DumpDNS.Internal;

namespace DumpDNS.Test;

public class Utils
{
    [Fact]
    public void Test_Valid_Domain()
    {
        Assert.True(Internal.Utils.CheckValidDomain("wtdawson.info"));
        Assert.True(Internal.Utils.CheckValidDomain("a.wtdawson.info"));
        Assert.True(Internal.Utils.CheckValidDomain("a.bwtdawson.info"));

        Assert.False(Internal.Utils.CheckValidDomain("wtdawson"));
        Assert.False(Internal.Utils.CheckValidDomain("wtdawson.info/abc"));
        Assert.False(Internal.Utils.CheckValidDomain("http://wtdawson.info"));
        Assert.False(Internal.Utils.CheckValidDomain("https://wtdawson.info"));
    }
}
