# GitHub User Activity CLI

A simple command-line interface (CLI) tool written in C# to fetch and display a GitHub user's recent activity using the GitHub API:
https://roadmap.sh/projects/github-user-activity 

---

## Features

- Fetch recent GitHub activity for any user.
- Supports multiple event types:
  - Pushes
  - Issues
  - Pull Requests
  - Stars (WatchEvent)
  - Forks
  - Releases
  - Comments on PRs
  - Branch/tag creations and deletions
- Displays a human-readable summary in the terminal.
- Limits output for readability (default: last 10 events).

---

## Prerequisites

- [.NET 6 SDK or later](https://dotnet.microsoft.com/download)
- Internet connection (to fetch data from GitHub API)

---

## Usage

1. Clone the repository:

```bash
git clone https://github.com/<your-username>/github-activity-cli.git
cd github-activity-cli

2. Build the project:

```bash
dotnet build

3. Run the CLI with a GitHub username:

```bash
dotnet run -- <username>

## Notes

- The CLI uses the GitHub Events API to fetch public user activity.
- Activity for private repositories will not be shown.
- The .vs folder and other IDE-specific files are ignored via .gitignore.
