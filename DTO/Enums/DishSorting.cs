using System.Text.Json.Serialization;

namespace DTO.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum DishSorting
    {
        NameAsc,
        NameDesc,
        PriceAsc,
        PriceDesc,
        RatingAsc,
        RatingDesc
    }
}
