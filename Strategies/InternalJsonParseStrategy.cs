using ContentParserApi.DTOs;
using ContentParserApi.Enums;
using System.Text.Json;

namespace ContentParserApi.Strategies
{
    public class InternalJsonParseStrategy : IParserStrategy
    {
        public CheckType Type => CheckType.InternalJson;
        public ResponseDto Parse(PayLoadDto payload)
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
