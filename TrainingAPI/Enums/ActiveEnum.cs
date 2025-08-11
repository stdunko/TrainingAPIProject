using System.Text.Json.Serialization;

namespace TrainingAPI.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ActiveEnum
    {
        True,
        False
    }
}
