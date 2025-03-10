using HotChocolateGraphQL.Backend;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddGraphQLServer()
				.AddQueryType<Query>();

var app = builder.Build();
app.MapGraphQL();

app.Run();

public partial class Program {}