using ContentParserApi.DTOs;
using ContentParserApi.Enums;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace ContentParserApi.Strategies
{
    public class CsvParseStrategy : IParserStrategy
    {
    public CheckType Type => CheckType.CSV;
       public ResponseDto Parse(PayLoadDto payload)
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
    }
}
