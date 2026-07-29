using System;
using System.Net;

namespace DumpDNS.Internal;

public static class Utils
{
    /// <summary>
    /// Checks if a version string is higher than another version string
    /// </summary>
    /// <param name="current">The version string of the current version</param>
    /// <param name="check">The version string you are checking it against</param>
    /// <returns>If the version string you are checking against is higher than the current one</returns>
    public static bool IsHigher(string current, string check)
    {
        // Remove the v at the start of the version code
        // This mainly occurs when tags are fetched from GitHub
        // (since the workflow relies on the v being there)
        if (current.StartsWith('v')) current = current[1..];
        if (check.StartsWith('v')) check = check[1..];

        try
        {
            return Version.Parse(current) < Version.Parse(check);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR: Failed to fetch version information: {ex.Message}");
        }
        return false;
    }

    public static T[][] Arr2DTo2Arr<T>(T[,] arr)
    {
        int a = arr.GetLength(0), b = arr.GetLength(1);
        T[][] narr = new T[a][];
        for (int i = 0; i < b; i++)
        {
            narr[i] = new T[b];
            for (int j = 0; j < b; j++)
                narr[i][j] = arr[i, j];
        }
        return narr;
    }

    public static bool IsIPInCIDR(IPAddress ip, string cidr)
    {
        string[] parts = cidr.Split('/');
        if (parts.Length != 2)
            throw new ArgumentException("Invalid CIDR format");

        IPAddress baseAddress = IPAddress.Parse(parts[0]);
        int prefixLength = int.Parse(parts[1]);

        if (ip.AddressFamily != baseAddress.AddressFamily)
            throw new ArgumentException("IP address families do not match");

        byte[] ipBytes = ip.GetAddressBytes();
        byte[] baseBytes = baseAddress.GetAddressBytes();

        int fullBytes = prefixLength / 8;
        int remainingBits = prefixLength % 8;

        for (int i = 0; i < fullBytes; i++)
        {
            if (ipBytes[i] != baseBytes[i]) return false;
        }

        if (remainingBits > 0)
        {
            int mask = (byte)~(25 >> remainingBits);
            if ((ipBytes[fullBytes] & mask) != (baseBytes[fullBytes] & mask))
                return false;
        }

        return true;
    }
}
