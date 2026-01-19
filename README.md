# OrderAgregatorAPI_MVP
MVP of an order aggregator based on a sample assignment.

## Assignment wording:

Design a web service that:
- Is written in the current version of .NET.
- Offers a RESP API endpoint to accept one or more orders in the format:
  ```json
  [
    {
      "productId": 456,
      "quantity": 5
    },
    {
      "productId": 789,
      "quantity": 42
    }
  ]
- Orders are aggregated for further processing - item counts are summed by product Id.
- Aggregated orders are sent to an internal system no more often than once every 20 seconds - for our purposes it is sufficient to just indicate this and print JSON to the console.
- The service should account for the possibility of a large number of small orders (hundreds per second) for a relatively limited number of product Ids (hundreds in total).
- The data persistence approach should be extensible and configurable - for our purposes, implementing in-memory data storage will be sufficient.
- The code should contain (at least some) tests.
- Try to propose other possible improvements and either implement them directly or just outline / describe them.
- Keep the code as you would imagine it in a production application.
- Preferably submit the code by publishing it on GitHub.

## A few words about the implementation:
- I created the project as an ASP.net Core WebAPI project with Swagger, it makes the API easy to test.
- This implementation can be downloaded, built, run, and tested.

## Implemented improvements:
- Configurable frequency of sending/printing aggregated item counts in appsettings.json (setting AggregatorFlushConfiguration.FlushIntervalSeconds)

## Possible further improvements:
- Instead of .net 8 it is possible to use .net 9, which includes performance and security improvements. I assumed an LTS version is used, which is currently (Q4 2025) .net 8. The upgrade to .net 9 should be quite straightforward.
- Some form of authentication/authorization could be added.
- A list of allowed product IDs could be stored somewhere (in DB / appsettings.json / Azure Key Vault) and only those allowed ones would be considered.
- If the input data contained something like orderId or requestId, redundant duplicate API calls could be ignored.
- Configurability of "resetting/not resetting" aggregated item counts after printing to the console / sending further.
- Rate Limiting could be set up to limit maximum API usage.
- Using FluentValidation instead of built-in model validation.
- The API method for POST /api/Orders could be async. Right now it doesn’t make much sense because it uses in-memory storage (a Dictionary), which is very fast and doesn’t involve I/O. It would make sense if the method called another API or persisted data to an MSSQL database, where you might need to await I/O operations.
