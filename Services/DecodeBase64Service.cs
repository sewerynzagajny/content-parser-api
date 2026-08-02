using ContentParserApi.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace ContentParserApi.Services
{
    public class DecodeBase64Service
    {


     private static string DecodeBase64Str(string base64Str){
            if (string.IsNullOrWhiteSpace(base64Str) || base64Str.Length % 4 != 0)
            {
                return base64Str;
            }
            Span<byte> buffer = stackalloc byte[base64Str.Length];
            if (Convert.TryFromBase64String(base64Str, buffer, out int bytesWritten)){
                return Encoding.UTF8.GetString(buffer.Slice(0, bytesWritten));
            } else {
            return base64Str;
            } 
    }

    public ResponseDto DecodeBase64(ResponseDto parsedData)
        {
            string copyParsedDataStr = JsonSerializer.Serialize(parsedData);
            JsonNode? copyParsedData = JsonSerializer.Deserialize<JsonNode>(copyParsedDataStr);
            JsonArray? copyDataProcessed = copyParsedData?["data_processed"]?.AsArray();

            if (copyDataProcessed != null) 
            {
                foreach (var element in copyDataProcessed)
                {
                    if (element == null) continue;
                    foreach (var item in element.AsObject().ToArray())
                    {
                        string  itemStr = item.Value?.ToString()?? string.Empty;
                        string decodeStr = DecodeBase64Str(itemStr);
                        if (itemStr != decodeStr){
                            element!.AsObject()[item.Key] = decodeStr;
                        }
                    }
                }
            } else 
                {
                throw new ArgumentException ("Data in data_processed is incorect");
                }
           string updateCopyParsedDataStr = copyParsedData?.ToJsonString() ?? string.Empty;
            if (string.IsNullOrEmpty(updateCopyParsedDataStr))
            {
                throw new FormatException("Failed to generate updated JSON string.");
            }
            ResponseDto ? response = JsonSerializer.Deserialize<ResponseDto>(updateCopyParsedDataStr);

            return response ?? throw new FormatException("Failed to deserialize updated JSON back to ResponseDto."); 
        }
    }
}
