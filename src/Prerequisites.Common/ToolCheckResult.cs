namespace Prerequisites.Common;

/// <summary>
/// The outcome of running an external tool to completion. A non-zero <see cref="ExitCode"/>
/// is a normal, expected outcome here — it is not distinguished by throwing.
/// </summary>
public sealed record ToolCheckResult(int ExitCode, string StandardOutput, string StandardError)
{
    public bool Succeeded => ExitCode == 0;

    /// <summary>Standard output and standard error concatenated, for tools that report version information on either stream.</summary>
    public string CombinedOutput => StandardOutput + StandardError;
}
