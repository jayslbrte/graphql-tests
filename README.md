# GRAPHQL XUNIT TESTS USING TestContainers

This is to showcase that graphqls can be tested as part of the ci pipeline using:
1. XUNIT - https://github.com/xunit/xunit
2. WebApplication Factory - https://www.codeproject.com/Articles/5377616/WebApplicationFactory-in-ASP-NET-Core-Practical-Ti
3. TestContainers - https://dotnet.testcontainers.org/



## How to run the graphql program

```bash
  cd ./HotChocolateGraphQL.Backend/
  dotnet clean
  dotnet restore
  dotnet build

```

run the graphql program
```bash
  dotnet run
```

##How to run the XUNIT Test
```bash
cd ./test_acceptance/
dotnet test
```