# trip-logger

Sample app for logging bicycling trips

## Prerequisites

This solution was built using the following.

- Visual Studio Community 2019 (V 16.4.2)
    - With the Azure Development workload
    - .Net Core 3
    - Azure Functions V3
- Cosmos DB Emulator

## Running the Solution

In order to run the project it is expected that you have the prerequisites installed on your machine and the Cosmos DB emulator is running with the default configuration.

The Cosmos DB connection string is stored in the local.settings.json file under the key `Values.CosmosDBConnection` and is set to the default value for the Cosmos DB emulator.

You can then run the `TripLoggerServices` project from Visual Studio to make the service available to make service calls (using F5 or Ctrl+F5).

Another option for starting the Azure Functions project is to execute `func start --build` from the command line inside the project folder `\src\TripLoggerServices`.

It is recommended to use a tool like Postman to make calls to the services.

There is a Postman collection in `\tests\Postman` with requests for each function. You will have to modify most requests with the generated tripId created after calling the `/trips` function.

The file `\documents\design.md` contains more details about each function and how to call them.
