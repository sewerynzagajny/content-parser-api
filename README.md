# ContentParserApi

A simple ASP.NET Core (`.NET 10`) API for parsing input content and returning a unified response format.

## What the application does

The API exposes one endpoint that accepts content in one of two formats:
- `CSV`
- `INTERNAL_JSON`

Then it:
1. parses the provided `content`,
2. builds a response with status and processed row count,
3. attempts to decode Base64 values inside `data_processed`.

## Endpoint

- **POST** `/api/v1/parse-content`
- **Content-Type:** `application/json`

### Request body

```json
{
  "type": "CSV",
  "content": "name,city\nSm9obiBEb2U=,V2Fyc2F3"
}
```

Supported `type` values:
- `CSV`
- `INTERNAL_JSON`

## Usage examples

### 1) CSV

#### Request

```json
{
  "type": "CSV",
  "content": "name,email\nSmFu,amFuQGV4YW1wbGUuY29t"
}
```

#### Sample response

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

### 2) INTERNAL_JSON

#### Request

```json
{
  "type": "INTERNAL_JSON",
  "content": "[{\"title\":\"SGVsbG8=\",\"value\":\"123\"}]"
}
```

#### Sample response

```json
{
  "status": "Success",
  "number_of_rows_processed": 1,
  "data_processed": [
	{
	  "title": "Hello",
	  "value": "123"
	}
  ]
}
```

## Run locally

From the project directory:

1. `dotnet restore`
2. `dotnet run`

Default URLs (from `launchSettings.json`):
- `http://localhost:5247`
- `https://localhost:7122`

Swagger UI:
- `http://localhost:5247/swagger`
- `https://localhost:7122/swagger`

## Project structure

- `Controllers/ParseContentController.cs` – API endpoint and error handling.
- `Services/ParseService.cs` – `CSV` and `INTERNAL_JSON` parsing logic.
- `Services/DecodeBase64Service.cs` – Base64 decoding attempt for output values.
- `DTOs/PayLoadDto.cs` – input model.
- `DTOs/ResponseDto.cs` – response model.
- `Enums/CheckType.cs` – allowed input types.

## Error responses

The API returns `status: "Error"` with a relevant message, for example when:
- request body format is invalid,
- input type is unsupported,
- `CSV` or `INTERNAL_JSON` structure is invalid,
- an unexpected server-side error occurs.

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
