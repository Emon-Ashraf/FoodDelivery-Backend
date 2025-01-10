using System.Text.Json.Serialization;

namespace DTO.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum OrderStatus
    {
        InProcess,
        Delivered
    }
}
