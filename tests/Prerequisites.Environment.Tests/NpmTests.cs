using Prerequisites.Common;
using Xunit;

namespace Prerequisites.Environment.Tests;

public class NpmTests
{
    [Fact(DisplayName = "TEST1: npm is on the PATH and reports its version.")]
    public void Npm_Invoked_ReportsVersion()
    {
        var result = ExternalToolRunner.Run("npm", "--version");

        Assert.True(result.Succeeded, $"'npm --version' exited with code {result.ExitCode}. Standard error:\n{result.StandardError}");
        Assert.True(result.StandardOutput.Trim().Length > 0, "npm ran but reported no version.");
    }
}
