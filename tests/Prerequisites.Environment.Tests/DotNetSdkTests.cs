using Prerequisites.Common;
using Xunit;

namespace Prerequisites.Environment.Tests;

/// <summary>
/// P709's two pre-shipped solutions, and every lab baseline built on top of them, target .NET 10.
/// </summary>
public class DotNetSdkTests
{
    private const int RequiredMajorVersion = 10;

    [Fact(DisplayName = "TEST1: dotnet CLI is on the PATH and reports at least one installed SDK.")]
    public void DotNetCli_Invoked_ReportsAtLeastOneSdk()
    {
        var result = RunListSdks();

        Assert.True(result.Succeeded, $"'dotnet --list-sdks' exited with code {result.ExitCode}. Standard error:\n{result.StandardError}");
        Assert.True(result.StandardOutput.Trim().Length > 0, "No .NET SDKs are installed.");
    }

    [Fact(DisplayName = "TEST2: at least one installed .NET SDK is version 10 or later.")]
    public void DotNetCli_ListSdks_ContainsSdkTenOrLater()
    {
        var result = RunListSdks();

        var hasRequiredSdk = result.StandardOutput
            .Split(System.Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Any(line => ToolVersion.MeetsMinimum(line, RequiredMajorVersion));

        Assert.True(hasRequiredSdk,
            $"No installed SDK is version {RequiredMajorVersion}.0 or later. Installed SDKs:\n{result.StandardOutput}\n" +
            "Install the .NET 10 SDK from https://dotnet.microsoft.com/download/dotnet/10.0 and re-run these tests.");
    }

    private static ToolCheckResult RunListSdks() => ExternalToolRunner.Run("dotnet", "--list-sdks");
}
