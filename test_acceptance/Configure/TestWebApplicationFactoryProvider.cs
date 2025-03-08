using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using test_acceptance.Configure.fixtures;
using Xunit;

namespace test_acceptance.Configure;

public class TestWebApplicationFactoryProvider (bool initMockoon = true) : IAsyncLifetime
{
    readonly MockoonFixture  _mockoonFixture = new ();

    public async Task InitializeAsync()
    {
        if (initMockoon)
        {
            await Task.WhenAll(
                _mockoonFixture.InitializeAsync()
                
            );
            await Task.Delay(5000);
        }
    }

    public async Task DisposeAsync()
    {
        if (initMockoon)
        {
            await Task.WhenAll(
                _mockoonFixture.DisposeAsync()
            );
        }
    }

    public TestWebAppFactory<Program> GetTestWebApplicationFactory()
    {
        var configureBuilder = ConfigureBuilderWithOpaSettings();
        return new TestWebAppFactory<Program>(configureBuilder);
    }

    Action<IHostBuilder> ConfigureBuilderWithOpaSettings()
    {
        return builder =>
        {
            builder.ConfigureLogging(logging =>
            {
                logging.SetMinimumLevel(LogLevel.Trace);
            });

            builder.ConfigureHostConfiguration(cfgBuilder =>
            {
                cfgBuilder.AddInMemoryCollection(
                [
                    
                ]);
            });
        };
    }

}