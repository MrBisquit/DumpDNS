using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DumpDNS.Functionality
{
    public static class Version
    {
        public static string CurrentVersion = "0.0.0"; // Changes automatically

        public static bool IsVisible = false; // False by default, wait until data is fetched
        public static string VersionString = "...";
        public static bool IsNewVersionAvailable = false;

        public static async void StartCheck()
        {
            CurrentVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            List<string> split = CurrentVersion.Split('.').ToList();
            split.RemoveAt(split.Count - 1);
            CurrentVersion = string.Join('.', split);

            IsVisible = true;
            GitHubClient client = new GitHubClient(new ProductHeaderValue("DumpDNS"));
            var release = await client.Repository.Release.GetLatest("MrBisquit", "DumpDNS");
            
            if(IsHigher(CurrentVersion, release.TagName))
            {
                VersionString = $"{CurrentVersion} < {release.TagName}";
                IsNewVersionAvailable = true;
            } else
            {
                VersionString = CurrentVersion;
            }

            Program.UpdateBottom(null, EventArgs.Empty);
        }

        /// <summary>
        /// Checks if a version string is higher than another version string
        /// </summary>
        /// <param name="current">The version string of the current version</param>
        /// <param name="check">The version string you are checking it against</param>
        /// <returns>If the version string you are checking against is higher than the current one</returns>
        public static bool IsHigher(string current, string check)
        {
            string[] currentSplit = current.Split(".");
            string[] checkSplit = check.Split('.');

            for (int i = 0; i < currentSplit.Length; i++)
            {
                if (int.Parse(currentSplit[i]) < int.Parse(checkSplit[i])) return true;
            }
            return false;
        }
    }
}
