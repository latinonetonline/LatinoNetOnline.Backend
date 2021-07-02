namespace LatinoNetOnline.Backend.Modules.Events.Core.Dto.Tokens
{
    record Token(string AccessToken, string RefreshToken, string TokenType, int ExpiresIn);

}
