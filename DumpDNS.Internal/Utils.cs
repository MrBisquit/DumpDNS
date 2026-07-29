using System;

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
}
