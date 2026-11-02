using System.Net.Http.Json;
using Xunit;

namespace Prerequisites.AspNetCoreStack.Tests;

/// <summary>
/// Section 3.1, Lab 1 wraps the NVX decoder's REST API in an HttpClient service class. No extra
/// package is needed for this — System.Net.Http.Json ships in the .NET 10 shared framework —
/// so this test only confirms that framework surface is actually present and usable.
/// </summary>
public class NvxRestIntegrationTests
{
    [Fact(DisplayName = "TEST1: HttpClient's JSON extension methods are available for the NVX decoder REST wrapper.")]
    public void HttpClient_Constructed_ExposesJsonExtensionMethods()
    {
        using var client = new HttpClient();

        // A compile-time reference to an extension method proves System.Net.Http.Json is present;
        // resolving the actual endpoint would need lab hardware, which this check does not assume.
        Func<HttpClient, Task<object?>> jsonGet = c => c.GetFromJsonAsync<object>("https://localhost/status-placeholder");

        Assert.True(jsonGet is not null, "HttpContentJsonExtensions.GetFromJsonAsync did not resolve.");
    }
}
