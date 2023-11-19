using System.Text.Json.Serialization;

namespace ECommers.Abstraction.Model
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ResultType
    {
        Ok,
        BadRequest,
        Unexpected,
        NotFound,
        Unauthorized,
        Invalid,
        NoContent,
        InvalidModel
    }
}
