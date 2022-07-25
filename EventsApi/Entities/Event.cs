namespace WebApi.Entities;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Converters;
using WebApi.Helpers;

public class Event
{
	[JsonPropertyName("id")]
	public long Id { get; set; }

	[JsonPropertyName("city")]
	public string? City { get; set; }

	[JsonPropertyName("start_date")]
	[JsonConverter(typeof(CustomDateTimeConverter))]
	public DateTime StartDate { get; set; }

	[JsonPropertyName("end_date")]
	[JsonConverter(typeof(CustomDateTimeConverter))]
	public DateTime EndDate { get; set; }

	[JsonPropertyName("price")]
	[JsonConverter(typeof(CustomPriceConverter))]
	public Decimal Price { get; set; }

	[JsonPropertyName("color")]
	public String? Color { get; set; }

	[JsonPropertyName("status")]
	public String? Status { get; set; }
}
