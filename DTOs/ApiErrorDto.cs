using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ContentParserApi.DTOs
{
    public class ApiErrorDto
    {
        [Required]
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("error_message")]
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
