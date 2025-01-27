
SecureMicroservices
A modular microservices project built with .NET 8 to demonstrate CRUD operations, authentication, and API gateway integration.

Overview
The SecureMicroservices project implements a complete microservices architecture, showcasing secure and scalable application design. It leverages modern technologies to provide efficient communication between services while ensuring user authentication and authorization.

Features
CRUD Operations: APIs for managing movies and other entities.
Authentication: Utilizes Duende IdentityServer for secure token-based authentication and authorization.
API Gateway: Implements Ocelot API Gateway for routing and centralized request handling.
In-Memory Database: Simplifies development by using an in-memory database for data storage.
Modular Design: Clean separation of concerns into four distinct projects.
Technologies Used
.NET 8
Duende IdentityServer
Ocelot API Gateway
In-Memory Database
Project Structure
Movies.Api: Contains APIs for CRUD operations and movie-related business logic.
Movies.Client: Frontend client built using ASP.NET Core MVC for user interaction with the API.
IdentityServer: Handles authentication and authorization via OAuth 2.0 and OpenID Connect.
ApiGateway: Acts as a reverse proxy to centralize requests and handle routing between microservices.
Future Enhancements
Migrate to a production-ready database (e.g., SQL Server).
Add unit and integration tests.
Implement containerization using Docker.
Integrate distributed logging and monitoring.
