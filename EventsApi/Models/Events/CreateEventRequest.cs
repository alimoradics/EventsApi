namespace WebApi.Models.Events;

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using WebApi.Entities;

public class UpdateEventRequest
{
    [Required]
    [JsonPropertyName("city")]
    public string? City { get; set; }

    [Required]
    [JsonPropertyName("start_date")]
    public string? StartDate { get; set; }

    [Required]
    [JsonPropertyName("end_date")]
    public string? EndDate { get; set; }

    [Required]
    [JsonPropertyName("price")]
    public string? Price { get; set; }

    [Required]
    [JsonPropertyName("color")]
    public string? Color { get; set; }

    [Required]
    [EnumDataType(typeof(Status))]
    [JsonPropertyName("status")]
    public string? Status { get; set; }
}
