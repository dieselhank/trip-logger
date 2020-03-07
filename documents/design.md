## TripLoggerServices Solution/Project

The `TripLoggerServices` project contains the implementation of the services for recording and retrieving bicycling trips. It uses Azure Functions for each endpoint and relies on Azure Cosmos DB for data persistence.

### Functions

#### LogTripRequest POST
This function is the first step in the interaction with the service. Requests are made via `POST` requests to the `/trips` endpoint.

A `JSON` body is expected with the `POST` of the form.
```JSON
{
	"date": "2020-01-02",
	"tripFrom": "Home",
	"tripTo": "SW",
	"description": "Bike Commute",
	"Distance": {
		"length": 5.6,
		"units": "Miles"
	}
}
```
On sucess the function returns a 200 status with a generated unique id for the request in the response body.

Internally the function will perform some simple validation of the request and generate the tripId for the submitted trip.

The request data is saved in the `TripLog` Cosmos DB in the `Trips` collection using the `TripEntry` DTO. The initially saved data is of the form:

```JSON
{
    "id": "103ca302-1d5c-462a-949e-6727203cbf04",
    "date": "2020-01-02T00:00:00",
    "tripFrom": "Home",
    "tripTo": "SW",
    "description": "Bike Commute",
    "distance": {
        "length": 5.6,
        "units": "Miles"
    }
}
```

## Unit Tests

The `TripLoggerServicesTests` project contains unit tests for the `TripLoggerServices` project.
