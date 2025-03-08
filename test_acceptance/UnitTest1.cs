using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Microsoft.AspNetCore.Mvc.Testing;
using Snapshooter.Xunit;
using test_acceptance.Configure;
using test_acceptance.data;
using Xunit.Abstractions;


namespace test_acceptance;

public class UnitTest1(ITestOutputHelper testOutputHelper) : IAsyncLifetime
{
    static TestWebApplicationFactoryProvider? _provider;
    static TestWebAppFactory<Program>? _factory;
    HttpClient? _client;

    public async Task InitializeAsync()
    {
        _provider = new TestWebApplicationFactoryProvider();
        await _provider.InitializeAsync();
        await Task.Delay(5000);

        _factory = _provider.GetTestWebApplicationFactory();
        _client = _factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
        
    }

    public async Task DisposeAsync()
    {
        if (_factory != null)
            await _factory.DisposeAsync();
        if (_provider != null)
            await _provider.DisposeAsync();
    }
    
    
    
    
    [Fact]
    public async Task Test1()
    {
        /*var graphQLCLient = new GraphQLHttpClient(new GraphQLHttpClientOptions
        {
            EndPoint = new  Uri("http://localhost:49117/graphql/")
        }, new NewtonsoftJsonSerializer());*/

        // string request = new GraphQLHttpRequest
        // {
        //     Query = @"query {
        //             books {
        //             title
        //             author {
        //             name
        //             }
        //             }
        //             }"
        // };

        /*var response =  await graphQLCLient.SendQueryAsync<BookQueryResponse>(request);*/

        if (_client != null)
        {
            var response = await GetGraphResponseAsync<BookQueryResponse>("BookQuery.graphql", _client, testOutputHelper);
        
            Snapshot.Match(response);
        }
    }

     static async Task<GraphQLResponse<T>> GetGraphResponseAsync<T>(
        string requestFile, 
        HttpClient client,
        ITestOutputHelper testOutputHelper)
    { using var graphClient = GetGraphClient(client);
      var query = await GetQueryTextAsync(requestFile);
      var result = await graphClient.SendQueryAsync<T>(new GraphQLRequest{ Query = query });
      testOutputHelper.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(result));
      
      return result;

    }
     
    static async Task<string> GetQueryTextAsync(string requestFile) => 
        await  File.ReadAllTextAsync(GetQueryPathName(requestFile));
    
     static GraphQLHttpClient GetGraphClient(HttpClient client) =>
        new( new GraphQLHttpClientOptions{ EndPoint = new Uri("http://localhost/graphql"), },
            new NewtonsoftJsonSerializer(),
            client); 
                

    
     static string GetQueryPathName(string fileName) =>
        Path.Combine(Directory.GetCurrentDirectory(), "queries",fileName);
    
    
}

