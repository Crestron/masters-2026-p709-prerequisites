using Microsoft.AspNetCore.SignalR.Client;
using Xunit;

namespace Prerequisites.AspNetCoreStack.Tests;

/// <summary>
/// Section 2 opens a SignalR connection from Svelte to the back-end hub for both request/response
/// (Lab 1) and server-initiated broadcast (Lab 2). Both labs need this package restored and compiled.
/// </summary>
public class SignalRPackageTests
{
    [Fact(DisplayName = "TEST1: Microsoft.AspNetCore.SignalR.Client restores and its HubConnectionBuilder compiles.")]
    public void HubConnectionBuilder_Constructed_BuildsAHubConnection()
    {
        var builder = new HubConnectionBuilder()
            .WithUrl("https://localhost/hub-placeholder");

        var connection = builder.Build();

        Assert.True(connection is HubConnection, "HubConnectionBuilder.Build() did not return a HubConnection.");
    }
}
