namespace aguasdem_ms_plataforma_rpa.Models;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

[Table("automatizaciones", Schema = "rpa")]
public class Automatizacion
{
    [Key]
    [Column("auto_id")]
    [JsonPropertyName("autoId")]
    public long AutoId { get; set; }

    [Column("codigo")]
    [JsonPropertyName("codigo")]
    public string Codigo { get; set; } = string.Empty;

    [Column("nombre")]
    [JsonPropertyName("nombre")]
    public string Nombre { get; set; } = string.Empty;

    [Column("descripcion")]
    [JsonPropertyName("descripcion")]
    public string? Descripcion { get; set; }

    [Column("tipo")]
    [JsonPropertyName("tipo")]
    public string Tipo { get; set; } = string.Empty;

    [Column("entorno")]
    [JsonPropertyName("entorno")]
    public string? Entorno { get; set; }

    [Column("version")]
    [JsonPropertyName("version")]
    public string? Version { get; set; }

    [Column("estado")]
    [JsonPropertyName("estado")]
    public string? Estado { get; set; }

    [Column("usua_crea")]
    public int UsuaCrea { get; set; }

    [Column("fech_crea")]
    public DateTime FechCrea { get; set; } = DateTime.UtcNow;

    [Column("usua_modi")]
    public int? UsuaModi { get; set; }

    [Column("fech_modi")]
    public DateTime? FechModi { get; set; }

    [JsonIgnore]
    public ConfiguracionProceso? Configuracion { get; set; }

    [JsonIgnore]
    public AppConfiguration? AppConfig { get; set; }
}
