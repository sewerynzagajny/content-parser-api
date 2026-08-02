using System.Text.Json.Serialization;

namespace ContentParserApi.Enums
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CheckType
    {
    CSV,
        [JsonStringEnumMemberName("INTERNAL_JSON")]
        InternalJson 
    }
}
