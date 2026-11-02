# masters-2026-p709-prerequisites

Automated checks that confirm your machine is ready for the P709 labs. Each Unit Test checks one tool or package, so a failing test shows you what to fix before class.

## Before you start

Install the following:

| Requirement | Version | Check with |
| --- | --- | --- |
| .NET SDK | 10 or later | `dotnet --list-sdks` |
| Node.js | 24.18 or later | `node --version` |
| npm | Included with Node.js | `npm --version` |
| Git | Any recent version | `git --version` |
| Backend IDE | JetBrains Rider 2026.2.1 or later, **or** Visual Studio 2026 or later | |
| UI editor | Visual Studio Code, recommended for the Svelte UI work | |

Your network must also be able to reach nuget.org and the npm registry.

The .NET 10 SDK includes the .NET 10 and ASP.NET Core runtimes that the backend work needs. The
tests don't check your IDE or editor, so check those versions yourself.

> **Install the tools first, then open your IDE.** An IDE that was already running won't see changes
> to your `PATH`, so restart it after installing anything.

## Run the tests in JetBrains Rider

1. Select **File > Open** and choose `Masters2026-P709-Prerequisites.sln`.
2. Wait for the NuGet restore to finish.
3. Select **Tests > Run All Tests from Solution**.
4. Review the results in the **Tests** tool window. If it doesn't open by itself, select
   **View > Tool Windows > Tests**.

To run part of the suite, right-click a project in the **Solution Explorer** and select
**Run Unit Tests**, or click the icon next to a test in the code editor.

## Run the tests in Visual Studio

1. Select **File > Open > Project/Solution** and choose `Masters2026-P709-Prerequisites.sln`.
2. Select **Build > Build Solution** (`Ctrl+Shift+B`). Tests appear in Test Explorer only after a
   successful build.
3. Select **Test > Test Explorer** (`Ctrl+E`, `T`).
4. Select **Test > Run All Tests** (`Ctrl+R`, `A`).

To run part of the suite, right-click a project, class, or test in **Test Explorer** and select
**Run**.

> Visual Studio 2022 is not supported for this solution, because targeting `net10.0` requires
> Visual Studio 2026 (version 18.0) or later.

## Reading the results

All 12 tests should pass. Numbering such as `TEST1` restarts in each test class, so use the project
and class to identify a test. On failure, the test's message usually explains what to install or
fix.

| Test project | Tests | What it checks |
| --- | --- | --- |
| `Prerequisites.Environment.Tests` | 6 | .NET SDK, Node.js, npm, and Git are installed and meet the minimum versions |
| `Prerequisites.AspNetCoreStack.Tests` | 3 | SignalR client, Swashbuckle, and HttpClient JSON support are available |
| `Prerequisites.CrestronControlSystem.Tests` | 2 | The Crestron SIMPL# Pro SDK restores and exposes the expected types |
| `Prerequisites.CrestronFrontend.Tests` | 1 | `npm install` succeeds for Svelte, Vite, CrComLib, and WebXPanel |

The front-end test runs a real `npm install`, so the first run can take a few minutes. Later runs
reuse the packages already downloaded. Run the tests before class, on a network that can reach
nuget.org and the npm registry, so that everything is downloaded in advance.

## Troubleshooting

| Problem | What to do |
| --- | --- |
| A tool "is not on the PATH, or is not installed" | Install the tool, confirm that it works in a new terminal, then restart your IDE. |
| Crestron package restore fails | Check [Crestron.SimplSharp.SDK.Program](https://www.nuget.org/packages/Crestron.SimplSharp.SDK.Program) for a newer version before suspecting your network. Crestron publishes new builds often. |
| `npm install` fails | Check that your network can reach the npm registry, including through any company proxy. |
| "Fixture directory not found" | Rebuild the solution so that the fixture is copied to the output folder. |
| No tests appear in Visual Studio | Build the solution, and check the **Error List** for restore or build errors. |
