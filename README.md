# ContentParserApi

ASP.NET Core Web API (`.NET 10`) for decoding Base64 payload content and parsing data in a unified response format.

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
2. Validate supported `type` (`CSV`, `INTERNAL_JSON`).
3. Decode `content` from Base64.
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

## Project structure

- `Controllers/ParseContentController.cs` - endpoint and flow orchestration.
- `Services/DecodeBase64Service.cs` - Base64 decode validation and conversion.
- `Services/ParseService.cs` - CSV and INTERNAL_JSON parsing.
- `GlobalExceptions/GlobalExceptionHandler.cs` - centralized exception mapping.
- `DTOs/PayLoadDto.cs` - input model.
- `DTOs/ResponseDto.cs` - success response model.
- `DTOs/ApiErrorDto.cs` - error response model.
- `Enums/CheckType.cs` - supported types.

## Why .NET 10

I selected **.NET 10** because it is a stable modern version with long-term support.

I intentionally did not target **.NET 8** for this project, because .NET 8 support ends on **November 10, 2026**.

## Technology stack

- ASP.NET Core Web API (`net10.0`)
- `CsvHelper`
- `System.Text.Json`
- OpenAPI/Swagger (`Microsoft.AspNetCore.OpenApi`, `Swashbuckle.AspNetCore`)

## Notes

This project was created as a recruitment assignment demonstrating:
- ASP.NET Core Web API
- DTO validation
- CSV parsing
- JSON parsing (with `System.Text.Json`)
- Base64 decoding
- Error handling

## Future improvements

- Unify model validation (`ModelState`) handling in the MVC pipeline to always return a consistent `ApiErrorDto` response format.
- Extend global exception mapping to provide clearer HTTP status codes and error messages for invalid input scenarios.
