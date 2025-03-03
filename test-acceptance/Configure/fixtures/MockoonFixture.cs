using System.ComponentModel;
using System.Reflection;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;

namespace test_acceptance.Configure.fixtures;

public class MockoonFixture : IAsyncLifetime
{
    private readonly DotNet.Testcontainers.Containers.IContainer _container;
    private readonly ushort _mockoonContainerPort;
    private ushort _mockoonHostPort;

    public MockoonFixture()
    {
        var dataPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        _mockoonContainerPort = 3001;
        _mockoonHostPort = 0;
        _container = new ContainerBuilder()
            .WithImage("mockoon/cli")
            .WithCleanUp(true)
            .WithBindMount(dataPath, "/data", AccessMode.ReadOnly)
            .WithCommand("--data", "/data/data.json")
            .WithPortBinding(_mockoonContainerPort, assignRandomHostPort: false)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(_mockoonContainerPort))
            .Build();
    }

    public async Task InitializeAsync()
    {
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(60));
        await _container.StartAsync(cts.Token)!;
        _mockoonHostPort = _container.GetMappedPublicPort(_mockoonContainerPort);
    }

    public async Task DisposeAsync() => await _container.DisposeAsync();

    public string GetAddress() => $"http://localhost:{_mockoonHostPort}/";
}