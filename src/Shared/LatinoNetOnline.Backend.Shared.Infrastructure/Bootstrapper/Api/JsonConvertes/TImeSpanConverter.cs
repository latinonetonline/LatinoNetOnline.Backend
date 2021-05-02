using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LatinoNetOnline.Backend.Shared.Infrastructure.Bootstrapper.Api.JsonConvertes
{
    public class TimeSpanConverter : JsonConverter<TimeSpan>
    {
        public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return TimeSpan.Parse(reader.GetString() ?? "00:00:00", CultureInfo.InvariantCulture);
        }

        public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(format: null, CultureInfo.InvariantCulture));
        }
    }
}
