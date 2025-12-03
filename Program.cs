using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gihub_Activity_Project
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: github-activity <username>");
                return;
            }
            var username = args[0];
            var url = $"https://api.github.com/users/{username}/events";

            using HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Add("User-Agent", "github-activity-cli");

            try
            {
                // Fetch events as JSON
                var events = await client.GetFromJsonAsync<List<JsonElement>>(url);

                if (events == null || events.Count == 0)
                {
                    Console.WriteLine("No activity found.");
                    return;
                }

                // For now, just print raw event types
                foreach (var ev in events)
                {                    
                    Console.WriteLine(GetEventDescription(ev));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching data: {ex.Message}");
            }
        }

        static string GetEventDescription(JsonElement ev)
        {
            var type = ev.GetProperty("type").GetString();
            var repo = ev.GetProperty("repo").GetProperty("name").GetString();

            switch (type)
            {
                case "PushEvent":
                    var commits = ev.GetProperty("payload").GetProperty("commits").GetArrayLength();
                    return $"Pushed {commits} commit{(commits > 1 ? "s" : "")} to {repo}";

                case "IssuesEvent":
                    var issueAction = ev.GetProperty("payload").GetProperty("action").GetString();
                    return $"{char.ToUpper(issueAction[0]) + issueAction.Substring(1)} an issue in {repo}";

                case "PullRequestEvent":
                    var prAction = ev.GetProperty("payload").GetProperty("action").GetString();
                    return $"{char.ToUpper(prAction[0]) + prAction.Substring(1)} a pull request in {repo}";

                case "WatchEvent":
                    return "Starred " + repo;

                case "ForkEvent":
                    var forkee = ev.GetProperty("payload").GetProperty("forkee").GetProperty("full_name").GetString();
                    return $"Forked {repo} to {forkee}";

                case "ReleaseEvent":
                    var releaseAction = ev.GetProperty("payload").GetProperty("action").GetString();
                    var releaseTag = ev.GetProperty("payload").GetProperty("release").GetProperty("tag_name").GetString();
                    return $"{char.ToUpper(releaseAction[0]) + releaseAction.Substring(1)} release {releaseTag} in {repo}";

                case "PullRequestReviewCommentEvent":
                    return $"Commented on a pull request in {repo}";

                case "CreateEvent":
                    var refType = ev.GetProperty("payload").GetProperty("ref_type").GetString();
                    var refName = ev.GetProperty("payload").GetProperty("ref").GetString();
                    return $"Created {refType} {refName} in {repo}";

                case "DeleteEvent":
                    var deletedType = ev.GetProperty("payload").GetProperty("ref_type").GetString();
                    var deletedName = ev.GetProperty("payload").GetProperty("ref").GetString();
                    return $"Deleted {deletedType} {deletedName} in {repo}";

                default:
                    return $"{type} on {repo}";
            }
        }


    }
}
