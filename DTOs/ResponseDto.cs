using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ContentParserApi.DTOs
{
    public class ResponseDto
    {
        [Required]
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("number_of_rows_processed")]
        public int NumberOfRowsProcessed { get; set; }

        [Required]
        [JsonPropertyName("data_processed")]
        public object ? DataProcessed { get; set; }
    }
}
