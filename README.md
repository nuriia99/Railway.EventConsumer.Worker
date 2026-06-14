# Railway.EventConsumer.Worker

Event consumer worker for Railway systems.

Project structure
- Railway.EventConsumer.WebApi: ASP.NET Core Web API project (entry point). Contains controllers, Program.cs and the OpenAPI/Scalar configuration for API documentation.
- Railway.EventConsumer.UnitTests: xUnit test project for unit tests.
- Cross-cutting / shared code: contains common utilities such as an error handler and result types (e.g. OperationResult) used to produce consistent API error responses.
- Directory.Packages.props: central package management for consistent NuGet package versions across projects.

Key features
- Uses a centralized error handler to convert unhandled exceptions into consistent HTTP responses and operation results.
- Uses OpenAPI with Scalar instead of Swagger to expose API documentation.

Quick start
1. Restore packages: `dotnet restore`
2. Build: `dotnet build`
3. Run the API: `dotnet run --project Railway.EventConsumer.WebApi`

After the API is running you can open the Scalar UI (OpenAPI viewer) at: `https://localhost:{port}/scalar` (or the path configured by MapScalarApiReference; check console output for the actual port).
