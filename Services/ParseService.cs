using ContentParserApi.DTOs;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Net.NetworkInformation;
using System.Text.Json;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ContentParserApi.Services
{
    public class ParseService
    {

        public ResponseDto CsvParse(PayLoadDto payload) 
        {

            try
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
                    throw new FormatException("CSV structure is incorrect");
                }

                foreach (dynamic item in contentParse)
                {
                    var columns = (IDictionary<string, object>)item;

                    if (columns.Values.Any(value => value == null || string.IsNullOrWhiteSpace(value.ToString())) || 
                    columns.Keys.Any(key => key == null || string.IsNullOrWhiteSpace(key.ToString()))) {
                        throw new FormatException("CSV structure is incorrect");
                    }
                }

                ResponseDto convert = new ResponseDto
                {
                    Status = "Success",
                    NumberOfRowsProcessed = contentParse.Count,
                    DataProcessed = contentParse
                };
                return convert;

            }
            catch (Exception ex) when (ex is CsvHelperException)
            {
                throw new FormatException("CSV structure is incorrect", ex);
            }
            catch (Exception ex) when (ex is not ArgumentException && ex is not FormatException)
            {
              
                throw new FormatException("Failed to parse CSV data.", ex);
            }

        }

        public ResponseDto InternalJsonParse(PayLoadDto payload)
        {

            try
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
                    if (parseTask != null) contentParse.Add(parseTask);
                }
                else
                {
                    throw new ArgumentException("Failed to parse INTERNAL_JSON data");
                }


                int numberOfRowsProcessed = 0;


                if (contentParse != null)
                {
                    numberOfRowsProcessed = contentParse.Count;
                }
                ResponseDto paresed = new ResponseDto
                {
                    Status = "Success",
                    NumberOfRowsProcessed = numberOfRowsProcessed,
                    DataProcessed = contentParse
                };
                return paresed;
            }
            catch (Exception ex) when (ex is not ArgumentException && ex is not FormatException)
            {
                throw new FormatException("INTERNAL_JSON structure is incorrect", ex);
            }
    
        }
    }
}
