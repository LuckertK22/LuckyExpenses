using System.Net;
using System.Text.Json.Serialization;

namespace LuckyExpenses.Shared.Common
{
    public class ApiResponse<T>(string message = "respuesta exitosa", T? data = default)
    {
        [JsonPropertyName("ok")]
        public bool Ok { get; set; } = true;

        [JsonPropertyName("message")]
        public string Message { get; set; } = message;

        [JsonPropertyName("data")]
        public T? Data { get; set; } = data;

        [JsonPropertyName("errors")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public object? Errors { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public HttpStatusCode HttpStatusCode = HttpStatusCode.OK;
    }
}
