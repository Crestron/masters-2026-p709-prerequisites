using System.Text.RegularExpressions;

namespace Prerequisites.Common;

/// <summary>
/// Extracts a dotted version number (e.g. "10.0.100" or "v22.11.0") from a tool's output and
/// compares it against a minimum. Parsing and comparison are kept together because neither
/// is useful to a caller on its own — a test only ever wants the yes/no answer.
/// </summary>
public static class ToolVersion
{
    private static readonly Regex VersionPattern = new(@"(\d+)\.(\d+)(?:\.(\d+))?", RegexOptions.Compiled);

    /// <summary>
    /// True when <paramref name="toolOutput"/> contains a version number whose major component
    /// is at least <paramref name="minimumMajor"/> (and, if supplied, whose minor component is at
    /// least <paramref name="minimumMinor"/> when the majors are equal).
    /// </summary>
    public static bool MeetsMinimum(string toolOutput, int minimumMajor, int minimumMinor = 0)
    {
        var match = VersionPattern.Match(toolOutput);
        if (!match.Success)
        {
            return false;
        }

        var major = int.Parse(match.Groups[1].Value);
        var minor = int.Parse(match.Groups[2].Value);

        return major > minimumMajor || (major == minimumMajor && minor >= minimumMinor);
    }

    /// <summary>The first dotted version number found in <paramref name="toolOutput"/>, or null if none was found.</summary>
    public static string? Extract(string toolOutput)
    {
        var match = VersionPattern.Match(toolOutput);
        return match.Success ? match.Value : null;
    }
}
