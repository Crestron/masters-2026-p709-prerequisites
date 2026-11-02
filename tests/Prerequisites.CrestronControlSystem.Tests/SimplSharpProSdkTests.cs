using Crestron.SimplSharp;
using Crestron.SimplSharpPro;
using Xunit;

namespace Prerequisites.CrestronControlSystem.Tests;

/// <summary>
/// Confirms the two namespaces every Crestron control-system program starts from are present:
/// Crestron.SimplSharp (the runtime, e.g. ErrorLog) and Crestron.SimplSharpPro (CrestronControlSystem
/// and the device classes built on it, including the NVX decoder integrated in Section 3).
/// </summary>
public class SimplSharpProSdkTests
{
    [Fact(DisplayName = "TEST1: Crestron.SimplSharpPro.CrestronControlSystem is a usable base class.")]
    public void CrestronControlSystem_Type_IsAPublicNonSealedClass()
    {
        // Fully qualified: this project's own namespace (Prerequisites.CrestronControlSystem.Tests)
        // contains a "CrestronControlSystem" segment, which C# resolves an unqualified reference to
        // ahead of the SDK's class of the same name (CS0118) — see Crestron.SimplSharpPro below.
        var controlSystemType = typeof(Crestron.SimplSharpPro.CrestronControlSystem);

        Assert.True(controlSystemType.IsPublic, "CrestronControlSystem is not public — the SDK version may be wrong.");
        Assert.True(!controlSystemType.IsSealed, "CrestronControlSystem is sealed — every control-system program subclasses it.");
    }

    [Fact(DisplayName = "TEST2: Crestron.SimplSharp.ErrorLog is available for the required error-handling calls.")]
    public void ErrorLog_Type_ExposesAnErrorMethod()
    {
        var errorLogType = typeof(ErrorLog);
        var errorMethod = errorLogType.GetMethod("Error", new[] { typeof(string), typeof(object[]) });

        Assert.True(errorMethod is not null, "ErrorLog.Error(string, params object[]) was not found.");
    }
}
