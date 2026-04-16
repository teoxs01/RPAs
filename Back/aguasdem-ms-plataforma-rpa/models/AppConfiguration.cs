namespace aguasdem_ms_plataforma_rpa.Models;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

[Table("app_configurations", Schema = "rpa")]
public class AppConfiguration
{
    [Key]
    [Column("id")]
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [Column("app_name")]
    [JsonPropertyName("appName")]
    public string AppName { get; set; } = string.Empty;

    [Column("user")]
    [JsonPropertyName("user")]
    public string User { get; set; } = string.Empty;

    [Column("password")]
    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;

    [Column("url")]
    [JsonPropertyName("url")]
    public string? Url { get; set; }

    [Column("status")]
    [JsonPropertyName("status")]
    public string Status { get; set; } = "ACTIVO";

    [Column("date")]
    [JsonPropertyName("date")]
    public DateTime Date { get; set; } = DateTime.UtcNow;

    [Column("auto_id")]
    [JsonPropertyName("autoId")]
    public long? AutoId { get; set; }

    [ForeignKey("AutoId")]
    [JsonIgnore]
    public Automatizacion? Automatizacion { get; set; }
}
