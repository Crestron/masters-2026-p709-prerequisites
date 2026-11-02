using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Prerequisites.Common;

/// <summary>
/// Launches an external tool and captures its output. This is the only class in the solution
/// that talks to <see cref="Process"/>, so every test project depends on one small, well-tested seam.
/// </summary>
public static class ExternalToolRunner
{
    /// <summary>
    /// Runs <paramref name="fileName"/> with <paramref name="arguments"/> and waits for it to exit.
    /// </summary>
    /// <exception cref="ArgumentException">A precondition was violated: no file name was supplied.</exception>
    /// <exception cref="ToolNotFoundException">The executable itself could not be found on the PATH.</exception>
    public static ToolCheckResult Run(string fileName, string arguments, TimeSpan? timeout = null, string? workingDirectory = null)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            throw new ArgumentException("A tool file name is required.", nameof(fileName));
        }

        if (workingDirectory is not null && !Directory.Exists(workingDirectory))
        {
            throw new DirectoryNotFoundException($"Working directory '{workingDirectory}' does not exist.");
        }

        var startInfo = new ProcessStartInfo(ResolveExecutable(fileName), arguments)
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            WorkingDirectory = workingDirectory ?? string.Empty
        };

        using var process = new Process { StartInfo = startInfo };

        try
        {
            process.Start();
        }
        catch (Win32Exception ex)
        {
            throw new ToolNotFoundException(fileName, ex);
        }

        // Drain both streams in the background so the timeout below covers the tool itself. Reading
        // synchronously would block until the tool exits (so a hung tool never times out), and reading
        // one stream to the end before the other can deadlock once the unread pipe's buffer fills.
        var standardOutput = process.StandardOutput.ReadToEndAsync();
        var standardError = process.StandardError.ReadToEndAsync();

        var exited = process.WaitForExit(timeout ?? TimeSpan.FromSeconds(30));
        if (!exited)
        {
            // Kill the whole tree: npm on Windows is cmd.exe running node, and a surviving child would
            // keep the pipes open.
            process.Kill(entireProcessTree: true);
            throw new TimeoutException($"'{fileName} {arguments}' did not exit within the allotted time.");
        }

        // The process has exited, so both streams are at end-of-file and these complete promptly.
        return new ToolCheckResult(process.ExitCode, standardOutput.GetAwaiter().GetResult(), standardError.GetAwaiter().GetResult());
    }

    /// <summary>
    /// Finds the full path of a bare command name on Windows before handing it to <see cref="Process"/>.
    /// Win32's CreateProcess (which <see cref="Process.Start()"/> calls into when
    /// <see cref="ProcessStartInfo.UseShellExecute"/> is false) only auto-appends ".exe" when a file
    /// name has no extension — it does not consult PATHEXT the way a real command prompt does. That
    /// is invisible for dotnet/node/git (each ships as a genuine .exe), but npm on Windows is only
    /// ever installed as "npm.cmd", so an unresolved "npm" would fail to start even though a delegate
    /// typing the same command into an actual terminal works fine. Searching PATH with PATHEXT here
    /// reproduces what that terminal does. Returns <paramref name="fileName"/> unchanged if it already
    /// has a path or extension, if nothing matches, or on non-Windows platforms.
    /// </summary>
    private static string ResolveExecutable(string fileName)
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows) || Path.IsPathRooted(fileName) || Path.HasExtension(fileName))
        {
            return fileName;
        }

        var extensions = (System.Environment.GetEnvironmentVariable("PATHEXT") ?? ".COM;.EXE;.BAT;.CMD")
            .Split(';', StringSplitOptions.RemoveEmptyEntries);
        var directories = (System.Environment.GetEnvironmentVariable("PATH") ?? string.Empty)
            .Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries);

        foreach (var directory in directories)
        {
            foreach (var extension in extensions)
            {
                var candidate = Path.Combine(directory, fileName + extension);
                if (File.Exists(candidate))
                {
                    return candidate;
                }
            }
        }

        return fileName;
    }
}
