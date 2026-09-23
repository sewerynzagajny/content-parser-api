# ContentParserApi

ASP.NET Core Web API (`.NET 10`) for decoding Base64 payload content and parsing data in a unified response format.

## Refactor update (Strategy + Factory-style parser selection)

This version introduces a parser refactor focused on extensibility and cleaner startup configuration.

### What changed

1. **Parser Strategy pattern added**
   - `Strategies/IParserStrategy.cs` defines a common parser contract.
   - `Strategies/CsvParseStrategy.cs` handles `CSV` payloads.
   - `Strategies/InternalJsonParseStrategy.cs` handles `INTERNAL_JSON` payloads.

2. **Factory-style parser resolution added in `ParseService`**
   - `Services/ParseService.cs` now receives all registered `IParserStrategy` implementations.
   - It selects the correct parser by `CheckType` and delegates parsing.
   - Result: no parser-specific branching in controller logic.

3. **`Program.cs` refactored (slim startup)**
   - Startup/pipeline responsibilities were moved into extension methods under `ProgramSettings`:
	 - `ServiceCollectionExtensions.cs`
	 - `ControllerExtensions.cs`
	 - `WebApplicationExtensions.cs`
   - `Program.cs` now stays minimal and focused on app bootstrap.

4. **Project structure updated**
   - New folders introduced:
	 - `Strategies/`
	 - `ProgramSettings/`
	 - `Validations/`

## Overview

The API exposes one endpoint:
- **POST** `/api/v1/parse-content`
- **Content-Type:** `application/json`

Request payload format:

```json
{
  "type": "CSV" | "INTERNAL_JSON",
  "content": "..."
}
```

`content` must be a Base64 string. After decoding, the API parses the decoded text according to `type`.

## Processing flow

1. Validate request model.
2. Decode `content` from Base64.
3. Resolve parser strategy by payload `type`.
4. Parse decoded content:
   - `CSV` -> collection of row objects
   - `INTERNAL_JSON` -> validated JSON object/array
5. Return unified success response.
6. Handle errors globally via exception handler.

## Sample requests

### CSV

```json
{
  "type": "CSV",
  "content": "bmFtZSxlbWFpbApKYW4samFuQGV4YW1wbGUuY29t"
}
```

(Base64 decoded value: `name,email\nJan,jan@example.com`)

### INTERNAL_JSON

```json
{
  "type": "INTERNAL_JSON",
  "content": "W3sidGl0bGUiOiJIZWxsbyIsInZhbHVlIjoiMTIzIn1d"
}
```

(Base64 decoded value: `[{"title":"Hello","value":"123"}]`)

## Response format

### Success

```json
{
  "status": "Success",
  "number_of_rows_processed": 1,
  "data_processed": [
	{
	  "name": "Jan",
	  "email": "jan@example.com"
	}
  ]
}
```

### Error

```json
{
  "status": "Error",
  "error_message": "Invalid base64 string"
}
```

## Run locally

From the project directory:

1. `dotnet restore .\ContentParserApi.sln`
2. `dotnet run`

Default URLs (from `launchSettings.json`):
- `http://localhost:5247`
- `https://localhost:7122`

Swagger UI:
- `http://localhost:5247/swagger`
- `https://localhost:7122/swagger`

## Current project structure

- `Controllers/ParseContentController.cs` - endpoint and request flow orchestration.
- `Services/DecodeBase64Service.cs` - Base64 decode validation and conversion.
- `Services/ParseService.cs` - strategy resolution and parser dispatch.
- `Strategies/IParserStrategy.cs` - parser abstraction.
- `Strategies/CsvParseStrategy.cs` - CSV parser implementation.
- `Strategies/InternalJsonParseStrategy.cs` - INTERNAL_JSON parser implementation.
- `ProgramSettings/ServiceCollectionExtensions.cs` - service registration composition.
- `ProgramSettings/ControllerExtensions.cs` - controller/API behavior configuration.
- `ProgramSettings/WebApplicationExtensions.cs` - middleware and endpoint mapping.
- `GlobalExceptions/GlobalExceptionHandler.cs` - centralized exception mapping.
- `DTOs/PayLoadDto.cs` - input model.
- `DTOs/ResponseDto.cs` - success response model.
- `DTOs/ApiErrorDto.cs` - error response model.
- `Enums/CheckType.cs` - supported types.
- `Validations/` - application-level request validation.

## Technology stack

- ASP.NET Core Web API (`net10.0`)
- `CsvHelper`
- `System.Text.Json`
- OpenAPI/Swagger (`Microsoft.AspNetCore.OpenApi`, `Swashbuckle.AspNetCore`)

## Notes

This project was created as a recruitment assignment demonstrating:
- ASP.NET Core Web API
- DTO validation
- Strategy pattern for parser extensibility
- CSV parsing
- JSON parsing (with `System.Text.Json`)
- Base64 decoding
- Error handling

## Future improvements

- Add tests for strategy selection and parser behavior.
- Extend global exception mapping to provide clearer HTTP status codes and error messages for invalid input scenarios.
- Add support for CSV input with and without headers.
