using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Snapshooter.Xunit;
using test_acceptance.data;


namespace test_acceptance;

public class UnitTest1
{
    [Fact]
    public async Task Test1()
    {
        var graphQLCLient = new GraphQLHttpClient(new GraphQLHttpClientOptions
        {
            EndPoint = new  Uri("http://localhost:49117/graphql/")
        }, new NewtonsoftJsonSerializer());

        var request = new GraphQLHttpRequest
        {
            Query = @"query {
                    books {
                    title
                    author {
                    name
                    }
                    }
                    }"
        };

        var response =  await graphQLCLient.SendQueryAsync<BookQueryResponse>(request);
        Snapshot.Match(response);
    }
    
    
}

