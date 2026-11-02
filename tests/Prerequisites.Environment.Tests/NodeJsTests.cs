using Prerequisites.Common;
using Xunit;

namespace Prerequisites.Environment.Tests;

/// <summary>
/// The Svelte front-end (Vite 6+, the CH5 component library) needs a current Node.js LTS.
/// </summary>
public class NodeJsTests
{
    private const int MinimumMajorVersion = 24;
    private const int MinimumMinorVersion = 18;

    [Fact(DisplayName = "TEST1: node is on the PATH and reports its version.")]
    public void Node_Invoked_ReportsVersion()
    {
        var result = ExternalToolRunner.Run("node", "--version");

        Assert.True(result.Succeeded, $"'node --version' exited with code {result.ExitCode}. Standard error:\n{result.StandardError}");
    }

    [Fact(DisplayName = "TEST2: the installed Node.js version is at least the minimum the front-end tooling needs.")]
    public void Node_Version_MeetsMinimum()
    {
        var result = ExternalToolRunner.Run("node", "--version");

        Assert.True(ToolVersion.MeetsMinimum(result.StandardOutput, MinimumMajorVersion, MinimumMinorVersion),
            $"Node.js {ToolVersion.Extract(result.StandardOutput) ?? "(unknown)"} was found; version {MinimumMajorVersion}.{MinimumMinorVersion} or later is required. " +
            "Install a current LTS release from https://nodejs.org and re-run these tests.");
    }
}
