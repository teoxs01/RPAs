namespace aguasdem_ms_plataforma_rpa.Models;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

[Table("configuracion_procesos_automatizacion", Schema = "rpa")]
public class ConfiguracionProceso
{
    [Key]
    [Column("id")]
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [Column("nombre")]
    [JsonPropertyName("nombre")]
    public string Nombre { get; set; } = string.Empty;

    [Column("script_path")]
    [JsonPropertyName("scriptPath")]
    public string ScriptPath { get; set; } = string.Empty;

    [Column("hora_ejecucion")]
    [JsonPropertyName("horaEjecucion")]
    public TimeSpan HoraEjecucion { get; set; }

    [Column("dias_semana")]
    [JsonPropertyName("diasEjecucion")]
    public List<string> DiasSemana { get; set; } = new();

    [Column("estado")]
    [JsonPropertyName("activo")]
    public bool Estado { get; set; }

    [Column("usua_crea")]
    public int? UsuaCrea { get; set; }

    [Column("fech_crea")]
    public DateTime FechCrea { get; set; } = DateTime.UtcNow;

    [Column("usua_modi")]
    public int? UsuaModi { get; set; }

    [Column("fech_modi")]
    public DateTime? FechModi { get; set; }
}