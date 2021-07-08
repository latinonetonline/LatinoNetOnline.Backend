using System.Text.Json.Serialization;

namespace LatinoNetOnline.Backend.Modules.Events.Core.Dto.Tokens
{
    record Token([property: JsonPropertyName("access_token")] string AccessToken,
        [property: JsonPropertyName("refresh_token")] string RefreshToken,
        [property: JsonPropertyName("token_type")] string TokenType,
        [property: JsonPropertyName("expire_in")] int ExpiresIn);

}
