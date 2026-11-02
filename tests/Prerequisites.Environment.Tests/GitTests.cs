using Prerequisites.Common;
using Xunit;

namespace Prerequisites.Environment.Tests;

/// <summary>
/// The class repository is pulled branch-by-branch as each lab starts, so git must work
/// before the first lab, not be discovered missing during it.
/// </summary>
public class GitTests
{
    [Fact(DisplayName = "TEST1: git is on the PATH and reports its version.")]
    public void Git_Invoked_ReportsVersion()
    {
        var result = ExternalToolRunner.Run("git", "--version");

        Assert.True(result.Succeeded, $"'git --version' exited with code {result.ExitCode}. Standard error:\n{result.StandardError}");
        Assert.Contains("git version", result.StandardOutput, StringComparison.OrdinalIgnoreCase);
    }
}
