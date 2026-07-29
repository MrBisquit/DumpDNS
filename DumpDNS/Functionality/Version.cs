using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Octokit;

namespace DumpDNS.Functionality
{
    public static class Version
    {
        public static string CurrentVersion = "0.0.0"; // Changes automatically

        public static bool IsVisible = false; // False by default, wait until data is fetched
        public static string VersionString = "...";
        public static bool IsNewVersionAvailable = false;
        public static bool Unreleased = false;

        public static async Task StartCheck()
        {
            CurrentVersion = Assembly.GetExecutingAssembly().GetName().Version!.ToString();
            List<string> split = CurrentVersion.Split('.').ToList();
            split.RemoveAt(split.Count - 1);
            CurrentVersion = string.Join('.', split);

            IsVisible = true;
            GitHubClient client = new GitHubClient(new ProductHeaderValue("DumpDNS"));
            var release = await client.Repository.Release.GetLatest("MrBisquit", "DumpDNS");

            if (IsHigher(CurrentVersion, release.TagName))
            {
                VersionString = $"{CurrentVersion} < {release.TagName}";
                IsNewVersionAvailable = true;
            }
            else if (IsHigher(release.TagName, CurrentVersion))
            {
                VersionString = $"Unreleased ({CurrentVersion})";
                Unreleased = true;
            }
            else
            {
                VersionString = CurrentVersion;
            }

            if (Program.UpdateBottom != null) Program.UpdateBottom(null, EventArgs.Empty);
        }

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
            if (current.StartsWith("v")) current = current.Substring(1);
            if (check.StartsWith("v")) check = check.Substring(1);

            try
            {
                return System.Version.Parse(current) < System.Version.Parse(check);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: Failed to fetch version information: {ex.Message}");
            }
            return false;
        }
    }
}
