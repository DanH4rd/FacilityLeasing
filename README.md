# Facility Leasing 

Service for hosting process equipment for production facilities.
A customer owns a large number of production facilities and plans to lease out the areas of these facilities for hosting process equipment. In this context, there is a need for a service that would allow administering contracts for the placement of equipment on premises.
Technical aspects:
* .NET 9 Web minimal API.
* Custom authentication and authorization based on a custom request header.
* MS SQL Server is used as a database management system for storing data.
* Entity Framework with the Code First approach is used for database access.
* Fluent Validation is applied to data models validation.
* Problem Details and Exception handler are used for error handling and reporting.
* CQRS pattern is implemented using the MediatR library.
* The background processing of a contract creation is implemented with ASP.NET Core BackgroundService.

The service uses data from environment variables, described in the launchSettings.json below. The values provided in this file are for local run only. For production and other environments these variables should be configured separately.

    "ASPNETCORE_ENVIRONMENT": "Development",
    "DB_SERVER": "localhost",
    "DB_PORT": "1433",
    "DB_USER": "sa",
    "DB_PASSWORD": "Rpassw!1",
    "DB_NAME": "FacilityLeasing",
    "X_AUTH_HEADER": "a8ee3b24-2cf1-49fd-95cd-61874d340734"

# Contracts API

The API provides methods for creating a placement contract and reading list of contracts. Each contract has the following attributes: production facility code, process equipment code and number of equipment units within the contract.

The Swagger is enabled to explore and try the API endpoints: ` http://<server>/swagger `

The ‘X-AUTH’ header should be added to each request for authorization. Use the ‘Authorize’ button on the swagger page to automatically add this header to each request.

Sample request to get contracts list:

    curl -X 'GET' \
    'http://localhost:5291/contracts' \
    -H 'X-AUTH: a8ee3b24-2cf1-49fd-95cd-61874d340734'

Sample of the response:

    [
    {
        "facilityCode": "FAC001",
        "equipmentCode": "EQP001",
        "equipmentQuantity": 100
    },
    {
        "facilityCode": "FAC001",
        "equipmentCode": "EQP002",
        "equipmentQuantity": 50
    },
    {
        "facilityCode": "FAC001",
        "equipmentCode": "EQP004",
        "equipmentQuantity": 100
    }
    ]

Sample request to create a contract:

    curl -X 'POST' \
    'http://localhost:5291/contracts' \
    -H 'X-AUTH: a8ee3b24-2cf1-49fd-95cd-61874d340734' \
    -H 'Content-Type: application/json' \
    -d '{
    "facilityCode": "FAC004",
    "equipmentCode": "EQP008",
    "equipmentQuantity": 11
    }'

Sample of create contract response:
Http code 201

    {
    "productionFacilityId": 4,
    "processEquipmentId": 8,
    "equipmentUnits": 11,
    "productionFacility": null,
    "processEquipment": null,
    "id": 19,
    "createdDate": "2025-01-22T12:17:32.0030149Z",
    "isActive": true
    }

# Tests

xUnit is used as an unit and integration test platform, In-memory database is used. 






