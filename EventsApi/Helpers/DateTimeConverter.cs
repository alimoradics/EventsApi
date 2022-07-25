using System.Text.Json;
using System.Text.Json.Serialization;
using System.Globalization;

namespace WebApi.Helpers;

public class CustomDateTimeConverter : JsonConverter<DateTime>
{
	private readonly string _format;
	public CustomDateTimeConverter()
	{
		_format = "M/d/yyyy";
	}
	public override void Write(Utf8JsonWriter writer, DateTime date, JsonSerializerOptions options)
	{
		writer.WriteStringValue(date.ToString(_format, CultureInfo.InvariantCulture));
	}
	public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		return DateTime.ParseExact(reader.GetString(), _format, null);
	}
}
