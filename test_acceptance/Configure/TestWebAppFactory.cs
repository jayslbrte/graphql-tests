using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace test_acceptance.Configure;

public class TestWebAppFactory<TProgram>(Action<IHostBuilder> configureBuilder) : WebApplicationFactory <TProgram> where TProgram: class
{
 public IConfiguration Configuration { get; private set; } = null!;
 public IHost? _host;

 protected override IHost CreateHost(IHostBuilder builder)
 {
  configureBuilder?.Invoke(builder);
  builder.ConfigureAppConfiguration(config =>
  {
   Configuration = config.Build();
  });
  _host = base.CreateHost(builder);
  return _host;
 }

 
 public override async ValueTask DisposeAsync()
 {
  if (_host != null)
  {
   using var scope = _host.Services.CreateScope();
   var lifetime = scope.ServiceProvider.GetRequiredService<IHostApplicationLifetime>();
   
   lifetime.StopApplication();

   var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
   await _host.StopAsync(cts.Token);
  }
  await base.DisposeAsync();
 }
}