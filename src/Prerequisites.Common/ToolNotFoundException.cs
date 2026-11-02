namespace Prerequisites.Common;

/// <summary>
/// Thrown when an external tool (dotnet, node, npm, git, ...) cannot be started at all,
/// as distinct from the tool starting and reporting a failure of its own.
/// </summary>
public sealed class ToolNotFoundException : Exception
{
    public string ToolName { get; }

    public ToolNotFoundException(string toolName, Exception? innerException = null)
        : base($"'{toolName}' is not on the PATH, or is not installed. Install it and re-open the terminal before re-running these checks.", innerException)
    {
        ToolName = toolName;
    }
}
