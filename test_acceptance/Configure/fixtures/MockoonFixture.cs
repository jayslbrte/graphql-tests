

using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using HotChocolate;
using Xunit;
using Path = System.IO.Path;


namespace test_acceptance.Configure.fixtures;

public class MockoonFixture : IAsyncLifetime
{
    readonly IContainer _container;
    readonly ushort _mockoonContainerPort;
    ushort _mockoonHostPort;
    

    public MockoonFixture()
    {
        var dataPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        _mockoonContainerPort = 3000;
        _mockoonHostPort = 0;
        _container = new ContainerBuilder()
            .WithImage("mockoon/cli")
            .WithCleanUp(true)
            .WithBindMount(dataPath, "/data", AccessMode.ReadOnly)
            .WithCommand("--data", "/data/data.json")
            .WithPortBinding(_mockoonContainerPort, assignRandomHostPort: true)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(_mockoonContainerPort))
            .Build();
    }

    public async Task InitializeAsync()
    {
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(60));
        await _container.StartAsync(cts.Token)!;
        await Task.Delay(5000);
        _mockoonHostPort = _container.GetMappedPublicPort(_mockoonContainerPort);
    }

    public async Task DisposeAsync() => await _container.DisposeAsync();

    public string GetAddress() => $"http://localhost:{_mockoonHostPort}/";
}