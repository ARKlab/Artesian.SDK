# GitHub Copilot Instructions for Artesian.SDK

## Project Overview

Artesian.SDK is a .NET library that provides a client SDK for the Artesian API. The SDK represents the Artesian OpenAPI services 1:1 while additionally offering fluent business objects and query builders to enhance developer experience (DX) for core areas like reading and writing market data time series. This is a multi-framework library targeting net10.0, net8.0, netstandard2.1, netstandard2.0, and net462.

## Architecture

### Services (1:1 Mapping to Artesian OpenAPI)

The SDK has services that represent 1:1 the OpenAPI services exposed by Artesian. Their names end in "Service":

- **MarketDataService**: Manages market data metadata and write operations
  - OpenAPI spec: https://arkive.artesian.cloud/ArkDemo/swagger/docs/v2.1
- **QueryService**: Handles querying time series data
  - OpenAPI spec: https://arkive.artesian.cloud/ArkDemo/query/swagger/docs/v1.0
- **GMEPublicOfferService**: Manages GME public offer data
  - OpenAPI spec: https://arkive.artesian.cloud/ArkDemo/gmepublicoffer/swagger/docs/v2.0

**Service Responsibilities**:
- Manage client connection including serialization/deserialization (serde)
- Handle authentication (API-Key and OAuth Client Credentials)
- Manage error handling
- Implement retry policies, circuit breakers, and bulkhead isolation using Polly

### Fluent Business Objects (Enhanced DX)

The SDK additionally offers fluent business objects that enhance developer experience by:
- Helping create complex request payloads correctly
- Providing client-side validations
- Offering intuitive APIs for common operations

**Key Fluent Objects**:
- **Query Builders**: Fluent APIs for building queries (Actual, Versioned, Market Assessment, Auction)
- **Write Data Objects**: Fluent APIs for writing time series data (ActualTimeSerie, VersionedTimeSerie, MarketAssessment, Auction, BidAsk)

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
3. **Var Usage**: Prefer `var` for local variables
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

- **All public methods and DTOs must be well documented** by mimicking the OpenAPI specifications
- Add XML documentation comments for public APIs
- Use `#pragma warning disable CS1591` to suppress missing documentation warnings for internal enum values when appropriate
- Document parameters, return values, and exceptions
- Ensure documentation is comprehensive and matches the OpenAPI specs

### Commit Message Conventions

This project follows the [Conventional Commits](https://www.conventionalcommits.org/) specification for commit messages. This provides an easy set of rules for creating an explicit commit history and enables automated tooling.

#### Commit Message Format

Each commit message consists of a **header**, an optional **body**, and an optional **footer**:

```
<type>[optional scope]: <description>

[optional body]

[optional footer(s)]
```

The **header** is mandatory and must conform to the format above.

#### Types

Use one of the following types for each commit:

- **feat**: A new feature for the user or a particular subsystem
- **fix**: A bug fix for the user, not a fix to build scripts
- **docs**: Documentation only changes (README, XML docs, etc.)
- **style**: Changes that do not affect the meaning of the code (white-space, formatting, etc.)
- **refactor**: A code change that neither fixes a bug nor adds a feature
- **perf**: A code change that improves performance
- **test**: Adding missing tests or correcting existing tests
- **build**: Changes that affect the build system or external dependencies
- **ci**: Changes to CI configuration files and scripts
- **chore**: Other changes that don't modify src or test files
- **revert**: Reverts a previous commit

#### Scope

The scope is optional and should be the name of the affected component or module (e.g., `QueryService`, `MarketData`, `Authentication`, `GMEPublicOffer`).

#### Description

- Use the imperative, present tense: "change" not "changed" nor "changes"
- Don't capitalize the first letter
- No period (.) at the end
- Limit to 72 characters or less

#### Body

The body is optional and should include the motivation for the change and contrast this with previous behavior.

#### Footer

The footer is optional and should contain any information about **Breaking Changes** and reference GitHub issues that this commit closes.

**Breaking Changes** should start with the word `BREAKING CHANGE:` followed by a space or two newlines.

#### Examples

```
feat(QueryService): add support for auction time series queries

Add new query builder for auction data with support for filtering
by auction status and time range.

Closes #123
```

```
fix(Authentication): handle token refresh race condition

Multiple concurrent requests could cause duplicate token refresh
attempts. Added locking mechanism to ensure thread-safe refresh.
```

```
docs: update API examples in README

Add examples for new versioned time series query methods.
```

```
refactor(MarketData)!: change partition strategy interface

BREAKING CHANGE: IPartitionStrategy now requires additional method
GetPartitionCount(). Existing custom implementations must be updated.
```

## Building and Testing

- Build: `dotnet build`
- Test: `dotnet test`
- The project uses Directory.Build.props for shared MSBuild properties
- **All code paths must be tested** - comprehensive test coverage is required

## Versioning and API Compatibility

### API Version Strategy

- **SDK always uses the latest version of Artesian APIs**
- Services must stay in sync with the latest OpenAPI specifications

### Semantic Versioning

The SDK uses semantic versioning (MAJOR.MINOR.PATCH):

- **MAJOR**: Breaking changes
  - Changes that cause existing client code to break at compile time
  - Upgrading to a major version of the Artesian service API
- **MINOR**: Non-breaking API changes
  - Changes that require recompilation but no client code changes
  - Can change API in ways that still allow client code to compile cleanly
  - May change interface methods as long as client code compiles due to new extensions or defaults
  - Clients are not expected to provide their own implementations of interfaces
- **PATCH**: Assembly-identical changes
  - Can be upgraded without requiring recompilation
  - Bug fixes and internal improvements only

### API Design Guidelines

- **Be thoughtful in avoiding unneeded API breakage**
- **Prefer extension methods for new overloads in MINOR versions** to avoid growing interfaces
- Changing interface methods is acceptable in MINOR versions as long as:
  - Client code still compiles cleanly
  - New extensions or default parameters maintain compatibility
- Remember: clients don't implement SDK interfaces, so interface changes are less breaking than in typical library design

## Important Notes

1. The SDK provides both **read and write access** to the Artesian API with focus on excellent DX for core time series operations
2. Services (MarketDataService, QueryService, GMEPublicOfferService) are 1:1 representations of Artesian OpenAPI services
3. Market data is queried through different time series types: Actual, Versioned, Market Assessment, Auction, and BidAsk
4. Queries require Market Data IDs and time extraction windows (absolute or relative intervals)
5. Write operations use fluent objects (ActualTimeSerie, VersionedTimeSerie, etc.) for enhanced developer experience
6. The library supports multi-framework targeting - ensure code is compatible across all target frameworks
7. Use Polly policies for resilience: retry, circuit breaker, and bulkhead patterns
8. Partition strategies control how requests are batched/grouped

## When Suggesting Code

1. Prefer `var` for local variables
2. Include proper null checks and parameter validation using Guard clauses
3. Use NodaTime types for dates and times
4. Follow the established fluent API patterns for query builders and write objects
5. **Add comprehensive XML documentation for all public APIs** by mimicking OpenAPI specifications
6. Ensure compatibility with all target frameworks
7. Use async/await consistently for I/O operations
8. Follow the existing project structure and organization patterns
9. **Ensure all code paths are tested** with appropriate NUnit tests
10. When adding new features:
    - Consider versioning impact (MAJOR vs MINOR vs PATCH)
    - Prefer extension methods for new overloads in MINOR versions
    - Avoid unnecessary API breakage
11. Services should handle client connections, serialization, authentication, and error handling
12. Use fluent objects to enhance developer experience for complex operations
