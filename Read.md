
# REDACTED_PROJECT_NAME
This is the README file for the REDACTED_PROJECT_NAME project.
## Project Structure
OnlineStoreServices/
├── README.md
├── OnlineStoreServices.sln
├── src/
│   ├── OrderService/
│   │   ├── Core/
│   │   │   ├── Web.API/
│   │   │   │   ├── Controllers/
│   │   │   │   ├── Program.cs
│   │   │   │   └── appsettings.json
│   │   │   ├── Application/
│   │   │   ├── Domain/
│   │   │   ├── Infrastructure/
│   │   │   └── DependencyInjection/
│   │   ├── Shared/              ← OrderService-specific shared DTOs, utils, etc.
│   │   └── OrderService.csproj
│   │
│   ├── ProductCatalogService/
│   │   ├── Core/
│   │   │   ├── Web.API/
│   │   │   │   ├── Controllers/
│   │   │   │   ├── Program.cs
│   │   │   │   └── appsettings.json
│   │   │   ├── Application/
│   │   │   ├── Domain/
│   │   │   ├── Infrastructure/
│   │   │   └── DependencyInjection/
│   │   ├── Shared/              ← ProductCatalogService-specific shared DTOs, utils, etc.
│   │   └── ProductCatalogService.csproj


## Prerequisites
- .NET 8.0 SDK
- IDE such as Visual Studio or VS Code
- SQL Server or any other database supported by Entity Framework Core
- Postman / Swagger UI for testing

## Getting Started
1. Clone the repository:
   ```bash
   git clone
1. Navigate to the project directory:
   ```bash
   cd OnlineStoreServices
   ```
1. Restore dependencies:
   ```bash
   dotnet restore
   ```
1. Build the solution:
   ```bash
   dotnet build
   ```
1. Set up the database:
   - Update the connection strings in `appsettings.json` files located in each service's Web.API project.
   - Run the migrations to create the database schema:
	 ```bash
	 dotnet ef database update --project src/OrderService/Core/Infrastructure --startup-project src/OrderService/Core/Web.API
	 dotnet ef database update --project src/ProductCatalogService/Core/Infrastructure --startup-project src/ProductCatalogService/Core/Web.API
	 ```
	- Make sure the connection strings in appsettings.json point to your SQL Server instance.

## Running the Services
1. Navigate to each service's Web.API project directory and run the service:
   ```bash
   cd src/OrderService/Core/Web.API
   dotnet run
   ```
   In a new terminal, run the ProductCatalogService:
   ```bash
   cd src/ProductCatalogService/Core/Web.API
   dotnet run
   ```

## Testing the Services
- Use Postman or Swagger UI to test the API endpoints.
- Swagger UI is available at `http://localhost:{port}/swagger` for each service.
- Replace `{port}` with the actual port number on which the service is running.

## JWT Authentication
- The services use JWT for authentication.
- Configure JWT settings in the `appsettings.json` file of each service's Web.API project.
- Make sure to implement user authentication and token generation as needed.
- Include the JWT token in the Authorization header for secured endpoints:
  ```
  Authorization: Bearer {your_jwt_token}
  ```
- Replace `{your_jwt_token}` with the actual token obtained after user authentication.
- Ensure that the token is valid and not expired when making requests to secured endpoints.
- Adjust token expiration and signing settings in the JWT configuration as necessary for your application.
- Implement role-based access control if required by your application.
- Test the authentication flow thoroughly to ensure security.

## Idempotency
- Idempotency is implemented for critical operations to prevent duplicate processing.
- Use idempotency keys in the request headers for operations that require idempotency.
- Include the following header in your requests:
  ```
  Idempotency-Key: 123e4567-e89b-12d3-a456-426614174000
  ```
- replace the example key with a unique key for each operation.


## ProductCatalog API Endpoint
Method	URL	Role	Description

GET	/api/products	User/Admin	List products (Paging: ?pageNumber=1&pageSize=10)
GET	/api/products/{id}	User/Admin	Get product by ID
POST	/api/products	Admin	Create product
PUT	/api/products/{id}	Admin	Update product
DELETE	/api/products/{id}	Admin	Delete product
POST	/api/products/{id}/decrease-stock	Admin	Reduce product stock

## OrderService API Endpoint
Method	URL	Role	Description

POST	/api/orders	User/Admin	Create order (checks ProductCatalog: stock + IsActive)
GET	/api/orders/{id}	User/Admin	Get order details
GET	/api/orders/user	User	Get all orders for current user
POST	/api/orders/{id}/cancel	User	Cancel order (only Pending status)

## Running the Application

- Ensure both services are running as described in the "Running the Services" section.
- Use the provided API endpoints to interact with the services.
- Start ProductCatalog.API and Orders.API projects

- Open Swagger UI for each API

- Generate JWT token via /api/auth/token

- Authorize requests in Swagger

- Test CRUD and Orders workflow

## Contributing
- Contributions are welcome! Please fork the repository and create a pull request with your changes.