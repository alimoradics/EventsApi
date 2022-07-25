# EventsApi

This solution consits of three projects
1. EventsApi: contains the source code.
2. EventsApiTests: contains tests.
3. docker-compose: responsible for running docker.

## Run Projects

### EventsApi Project
1. Navigate into the directory _EventsApi_.
2. Run the command `dotnet run`.

Or open the project in Visual Studio and run the project.

Swagger UI is accessible through _https://localhost:<port>/swagger/index.html_.
Basic Authentication with username: `test` and password: `test` is needed to test the endpoints.

Or run `docker-compose up -d`

### EventsApiTests

1. Navigate to the project directory.
2. Run command `dotnet test`.


### docker-compose
Open the project in Visual Studio and run the project.

