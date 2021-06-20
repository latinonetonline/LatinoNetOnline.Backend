
using UAParser;

namespace LatinoNetOnline.Backend.Modules.Notifications.Core.Extensions
{
    static class UserAgentExtensions
    {
        public static string GetOperativeSystem(this string userAgent)
        {
            Parser uaParser = Parser.GetDefault();

            ClientInfo c = uaParser.Parse(userAgent);

            return $"{c.OS.Family} {c.OS.Major}.{c.OS.Minor}";
        }

        public static string GetDeviceName(this string userAgent)
        {
            Parser uaParser = Parser.GetDefault();

            ClientInfo c = uaParser.Parse(userAgent);

            return $"{c.Device.Family} ";
        }
    }
}
