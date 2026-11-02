using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Xunit;

namespace Prerequisites.AspNetCoreStack.Tests;

/// <summary>
/// Section 3.1's review step tests the back-end in isolation via Swagger UI before the front-end
/// is touched. Swashbuckle is what generates that UI's OpenAPI document.
/// </summary>
public class SwaggerUiPackageTests
{
    [Fact(DisplayName = "TEST1: Swashbuckle.AspNetCore restores and its SwaggerGen services register.")]
    public void AddSwaggerGen_Called_RegistersASwaggerGenerator()
    {
        // A bare ServiceCollection isn't enough here: Swashbuckle's generator depends on
        // IApiDescriptionGroupCollectionProvider (added by AddEndpointsApiExplorer) and, in turn,
        // on IWebHostEnvironment, which only a real host provides. WebApplicationBuilder is the
        // lightest thing that supplies both, and it mirrors how the course's own Program.cs wires
        // this up — this test builds the host but never starts it, so nothing actually listens.
        var builder = WebApplication.CreateBuilder();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        using var app = builder.Build();
        var generator = app.Services.GetService<ISwaggerProvider>();

        Assert.True(generator is not null, "AddSwaggerGen() did not register an ISwaggerProvider.");
    }
}
