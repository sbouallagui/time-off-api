# time-off-api
A rest API for managing employee leave requests.

## Project Overview
This API allows employees to:
- Submit leave requests
- Retrieve leave requests by ID
- Update request statuses

The project follows a clean, layered architecture using:
- .NET 9
- Dapper for data access
- PostgreSQL via Docker Compose
- Swagger for API documentation
- FluentValidation for input validation

## Project Structure

src/  
├── Time.Off.Api/ # Web API (controllers, DI, Swagger config)  
├── Time.Off.Application/ # Use cases, CQRS handlers  
├── Time.Off.Domain/ # Domain models, enums, value objects   
├── Time.Off.Infrastructure/ # Data access (Dapper repositories)

##  How to Run the Project

### Prerequisites:
- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- [Docker](https://www.docker.com/products/docker-desktop)

### Launch PostgreSQL Database (via Docker Compose)

From the project root (time-off-api) run:

```bash
cd database
docker-compose up -d
```
This will start a PostgreSQL container with the TimeOff database and initialize tables via the /docker-entrypoint-initdb.d scripts.

### Resetting the Database (Dev)
If you need to reset the PostgreSQL database and reload initial data (from init.sql), follow these steps:
- Remove existing containers and data volumes
```bash
docker-compose down -v
```
- Recreate the database and load initial data
```bash
docker-compose up -d
```
##  Run the API Locally

From the src/Time.Off.Api/ directory:

```bash
dotnet run --urls "https://localhost:5001"
```
The API will be available at:

```bash
https://localhost:5001/index.html
```
You’ll find interactive Swagger documentation with:

- Operation descriptions

- Example requests and responses

- HTTP status codes

## Run Tests
From the root folder, navigate to the tests folder first:

```bash
cd tests\Time.Off.UnitTests
dotnet test
```