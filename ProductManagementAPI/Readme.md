# ProductManagementAPI

ProductManagementAPI is a RESTful API for managing products. It provides endpoints for creating, reading, updating, and deleting products, as well as querying products by various criteria.

## Table of Contents

- [Features](#features)
- [Getting Started](#getting-started)
- [Running the Application](#running-the-application)
- [Running Tests](#running-tests)
- [API Endpoints](#api-endpoints)
- [Project Structure](#project-structure)

## Features

- Add, update, delete, and retrieve products
- Query products by category, name, and other criteria
- Get the total count of products
- Exception handling middleware for logging errors

## Getting Started

### Prerequisites

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (or use the in-memory database for testing)

### Installation

1. Clone the repository:

    ```sh
    git clone https://github.com/yourusername/ProductManagementAPI.git
    cd ProductManagementAPI
    ```

2. Restore the dependencies:

    ```sh
    dotnet restore
    ```

3. Update the database connection string in `appsettings.json`:

    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ProductManagementDB;Trusted_Connection=True;MultipleActiveResultSets=true"
    }
    ```

## Running the Application

1. Build the project:

    ```sh
    dotnet build
    ```

2. Run the application:

    ```sh
    dotnet run --project ProductManagementAPI
    ```

3. The API will be available at `https://localhost:5001` or `http://localhost:5000`.

## Running Tests

1. Navigate to the test project directory:

    ```sh
    cd ProductManagementAPI.Tests
    ```

2. Run the tests:

    ```sh
    dotnet test
    ```

## API Endpoints

### Products

- **GET /api/products**: Get all products
- **GET /api/products/{id}**: Get a product by ID
- **POST /api/products**: Add a new product
- **PUT /api/products/{id}**: Update a product
- **DELETE /api/products/{id}**: Delete a product
- **DELETE /api/products**: Delete all products
- **GET /api/products/category/{category}**: Get products by category
- **GET /api/products/name/{name}**: Get products by name
- **GET /api/products/sorted**: Get sorted products
- **GET /api/products/count**: Get the total count of products

## Project Structure

```plaintext
ProductManagementAPI/
    .sarif/
    .vs/
    appsettings.Development.json
    appsettings.json
    bin/
    Controllers/
    Data/
    Helpers/
    Models/
    obj/
    ProductManagementAPI.csproj
    ProductManagementAPI.sln
    Program.cs
    Properties/
    Services/
ProductManagementAPI.Tests/
    bin/
    obj/
    ProductManagementAPI.Tests.csproj
    ProductServiceTests.cs
ProjectManagement.sln
ProjectManagementClient/
    .angular/
    .editorconfig
    .gitignore
    .vscode/
    ...

Controllers: Contains the API controllers
Data: Contains the database context and migrations
Helpers: Contains helper classes and methods
Models: Contains the data models
Services: Contains the business logic
Tests: Contains the unit tests