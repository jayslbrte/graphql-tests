using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace test_acceptance.Configure.fixtures;

public class TestWebApplicationFactoryProvider (bool initMockoon = true) : IAsyncLifetime
{
    private readonly MockoonFixture  _mockoonFixture = new ();

    public async Task InitializeAsync()
    {
        if (initMockoon)
        {
            await Task.WhenAll(
                _mockoonFixture.InitializeAsync()
            );
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
        return new TestWebAppFactory<Program>(configureBuilder)
    }

    private Action<IHostBuilder> ConfigureBuilderWithOpaSettings()
    {
        return builder =>
        {
            builder.ConfigureLogging(logging =>
            {
                logging.SetMinimumLevel(LogLevel.Trace)
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