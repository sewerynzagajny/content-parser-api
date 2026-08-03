using ContentParserApi.DTOs;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Text.Json;

namespace ContentParserApi.Services
{
    public class ParseService
    {
        public ResponseDto CsvParse(PayLoadDto payload)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                DetectColumnCountChanges = true,
                IgnoreBlankLines = true
            };

            using var reader = new StringReader(payload.Content);
            using var csv = new CsvReader(reader, config);

            csv.Read();
            csv.ReadHeader();

            var contentParse = csv.GetRecords<dynamic>().ToList();

            if (contentParse.Count == 0)
            {
                throw new FormatException("Invalid CSV structure");
            }

            foreach (dynamic item in contentParse)
            {
                var columns = (IDictionary<string, object>)item;

                if (columns.Values.Any(value => value == null || string.IsNullOrWhiteSpace(value.ToString())) ||
                    columns.Keys.Any(key => key == null || string.IsNullOrWhiteSpace(key.ToString())))
                {
                    throw new FormatException("Invalid CSV structure");
                }
            }

            ResponseDto parsed = new ResponseDto
            {
                Status = "Success",
                NumberOfRowsProcessed = contentParse.Count,
                DataProcessed = contentParse
            };

            return parsed;
        }

        public ResponseDto InternalJsonParse(PayLoadDto payload)
        {
            using JsonDocument doc = JsonDocument.Parse(payload.Content);

            var contentParse = new List<object>();

            if (doc.RootElement.ValueKind == JsonValueKind.Array)
            {
                contentParse = JsonSerializer.Deserialize<List<object>>(payload.Content) ?? [];
            }
            else if (doc.RootElement.ValueKind == JsonValueKind.Object)
            {
                var parseTask = JsonSerializer.Deserialize<object>(payload.Content);
                if (parseTask != null)
                {
                    contentParse.Add(parseTask);
                }
            }
            else
            {
                throw new ArgumentException("Invalid INTERNAL_JSON structure");
            }

            int numberOfRowsProcessed = contentParse.Count;

            ResponseDto parsed = new ResponseDto
            {
                Status = "Success",
                NumberOfRowsProcessed = numberOfRowsProcessed,
                DataProcessed = contentParse
            };

            return parsed;
        }
    }
}
