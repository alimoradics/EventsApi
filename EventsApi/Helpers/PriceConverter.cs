using System.Text.Json;
using System.Text.Json.Serialization;

namespace WebApi.Helpers;

public class CustomPriceConverter : JsonConverter<Decimal>
{
	public CustomPriceConverter()
	{
	}
	public override void Write(Utf8JsonWriter writer, Decimal value, JsonSerializerOptions options)
	{
		writer.WriteStringValue(value.ToString());
	}
	public override Decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		return Convert.ToDecimal(reader.GetString());
	}
}
