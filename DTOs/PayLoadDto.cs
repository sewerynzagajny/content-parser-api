using ContentParserApi.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ContentParserApi.DTOs
{
    public class PayLoadDto
    {
        [Required]
        [JsonPropertyName("type")]
        public CheckType Type { get; set; }
        [Required]
        [JsonPropertyName("content")]
        public string Content { get; set; } = string.Empty;
        
    }
}
