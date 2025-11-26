# GitHub Copilot Instructions for Artesian.SDK

## Project Overview

Artesian.SDK is a .NET library that provides read access to the Artesian API for querying time series data including market data, assessments, and auction information. This is a multi-framework library targeting net10.0, net8.0, netstandard2.1, netstandard2.0, and net462.

## Key Technologies & Dependencies

- **Target Frameworks**: net10.0, net8.0, netstandard2.1, netstandard2.0, net462
- **HTTP Client**: Flurl.Http with Newtonsoft.Json serialization
- **Authentication**: Microsoft.Identity.Client for OAuth authentication
- **Date/Time**: NodaTime for date and time handling
- **Resilience**: Polly for retry policies, circuit breakers, and bulkhead isolation
- **Serialization**: MessagePack and Newtonsoft.Json with NodaTime support
- **Testing**: NUnit framework with Flurl.Http.Testing for mocking HTTP calls

## Coding Conventions

### C# Style Guidelines

1. **Indentation**: Use 4 spaces, not tabs
2. **Line Endings**: CRLF (Windows-style)
3. **Var Usage**: Avoid using `var` - explicitly declare types
4. **Braces**: Always use braces for control flow statements
5. **New Lines**: Open braces on new lines (Allman style)
6. **Null Checks**: Use nullable reference types (`#nullable enable`) where appropriate
7. **Expression-bodied Members**: Use for properties and accessors, avoid for methods and constructors
8. **Pattern Matching**: Prefer pattern matching over `is` with cast checks
9. **Modifier Order**: public, private, protected, internal, static, extern, new, virtual, abstract, sealed, override, readonly, unsafe, volatile, async

### Naming Conventions

- Use PascalCase for public members, types, and namespaces
- Use camelCase for private fields with underscore prefix (e.g., `_cfg`)
- Interface names start with 'I' (e.g., `IMarketDataService`)
- Async methods end with 'Async' suffix

### Code Organization

1. **Namespaces**: Organize code under `Artesian.SDK.*` namespace hierarchy
   - `Artesian.SDK.Common` - Shared utilities and helpers
   - `Artesian.SDK.Dto` - Data transfer objects
   - `Artesian.SDK.Factory` - Factory classes and builders
   - `Artesian.SDK.Service` - Service layer (Clients, Configuration, Query, MarketData, GMEPublicOffer)

2. **File Structure**: Use partial classes to separate concerns (e.g., `MarketDataService.cs`, `MarketDataService.ApiKey.cs`, `MarketDataService.Operations.cs`)

3. **Guard Clauses**: Use the internal `Guard` class for parameter validation with `CallerArgumentExpression`

### Testing Conventions

1. **Test Framework**: Use NUnit with `[TestFixture]` and `[Test]` attributes
2. **HTTP Mocking**: Use Flurl.Http.Testing's `HttpTest` for mocking HTTP calls
3. **Test Constants**: Store common test values in `TestConstants` class
4. **Test Naming**: Use descriptive names that indicate what is being tested
5. **Assertions**: Verify HTTP calls using Flurl's `ShouldHaveCalledPath` and related methods

### Time Series Query Patterns

When working with query builders, follow the fluent API pattern:
```csharp
var result = await qs.CreateActual()
    .ForMarketData(new int[] { 100000001 })
    .InGranularity(Granularity.Day)
    .InAbsoluteDateRange(new LocalDate(2018,08,01), new LocalDate(2018,08,10))
    .ExecuteAsync();
```

### Authentication & Configuration

- Support both API-Key and OAuth Client Credentials authentication
- Use `ArtesianServiceConfig` for service configuration
- Optionally configure policies with `ArtesianPolicyConfig` (retry, circuit breaker, bulkhead)

### Date/Time Handling

- Use NodaTime types (`LocalDate`, `LocalDateTime`, `Instant`, etc.) instead of built-in DateTime
- Use `Period` for time durations
- Use `RelativeInterval` and `Granularity` enums for time series queries

### Error Handling

- Use custom exception types where appropriate
- Validate parameters early using Guard clauses
- Provide meaningful error messages that include parameter names

### XML Documentation

- Add XML documentation comments for public APIs
- Use `#pragma warning disable CS1591` to suppress missing documentation warnings for internal enum values when appropriate
- Document parameters, return values, and exceptions

### Dependency Injection & Service Creation

- Services are created via constructors accepting configuration objects
- Support custom partition strategies (default: `PartitionByIDStrategy`)
- Allow policy configuration to be optional with sensible defaults

## Building and Testing

- Build: `dotnet build`
- Test: `dotnet test`
- The project uses Directory.Build.props for shared MSBuild properties

## Important Notes

1. This SDK focuses on **read access** to the Artesian API
2. Market data is queried through different time series types: Actual, Versioned, Market Assessment, and Auction
3. Queries require Market Data IDs and time extraction windows (absolute or relative intervals)
4. The library supports multi-framework targeting - ensure code is compatible across all target frameworks
5. Use Polly policies for resilience: retry, circuit breaker, and bulkhead patterns
6. Partition strategies control how requests are batched/grouped

## When Suggesting Code

1. Always use explicit types instead of `var`
2. Include proper null checks and parameter validation
3. Use NodaTime types for dates and times
4. Follow the established fluent API patterns for query builders
5. Add appropriate XML documentation for public APIs
6. Ensure compatibility with all target frameworks
7. Use async/await consistently for I/O operations
8. Follow the existing project structure and organization patterns
