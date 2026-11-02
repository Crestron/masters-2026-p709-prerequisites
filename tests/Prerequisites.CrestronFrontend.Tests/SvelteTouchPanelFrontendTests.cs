using Prerequisites.Common;
using Xunit;

namespace Prerequisites.CrestronFrontend.Tests;

/// <summary>
/// The prerequisite floor requires solid HTML/JavaScript, with Svelte "useful but not required" —
/// but the front-end throughout the day is Svelte, and CrComLib is described as "already wired"
/// in the baseline. This runs a real 'npm install' against a fixture manifest carrying the same
/// packages, so a delegate whose network blocks the npm registry finds out now, not mid-lab.
/// </summary>
public class SvelteTouchPanelFrontendTests
{
    private static string FixtureDirectory => Path.Combine(AppContext.BaseDirectory, "frontend-fixture");

    [Fact(DisplayName = "TEST1: npm install succeeds for Svelte, Vite, CrComLib and WebXPanel.")]
    public void NpmInstall_OnFrontendFixture_Succeeds()
    {
        if (!Directory.Exists(FixtureDirectory))
        {
            throw new DirectoryNotFoundException(
                $"Fixture directory not found at '{FixtureDirectory}'. Rebuild the project so package.json is copied to the output directory.");
        }

        var result = ExternalToolRunner.Run(
            "npm",
            "install --no-audit --no-fund",
            timeout: TimeSpan.FromMinutes(5),
            workingDirectory: FixtureDirectory);

        Assert.True(result.Succeeded,
            $"'npm install' failed with exit code {result.ExitCode} in '{FixtureDirectory}'.\n" +
            $"Standard error:\n{result.StandardError}\n" +
            "This usually means the npm registry is unreachable from this network, or a package version has been unpublished.");
    }
}
