namespace aguasdem_ms_plataforma_rpa.Models;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("ejecuciones", Schema = "rpa")]
public class EjecucionRpa
{
    [Key]
    [Column("ejec_id")]
    public long EjecId { get; set; }

    [Column("auto_id")]
    public long AutoId { get; set; }

    [Column("estado")]
    public string Estado { get; set; } = string.Empty;

    [Column("fecha_inicio")]
    public DateTime FechaInicio { get; set; }

    [Column("fecha_fin")]
    public DateTime? FechaFin { get; set; }

    [Column("duracion_ms")]
    public long? DuracionMs { get; set; }

    [Column("registros_procesados")]
    public int? RegistrosProcesados { get; set; }

    [Column("mensaje_error")]
    public string? MensajeError { get; set; }

    [Column("servidor")]
    public string? Servidor { get; set; }

    [Column("ambiente")]
    public string? Ambiente { get; set; }

    [Column("usua_crea")]
    public int UsuaCrea { get; set; }

    [Column("fech_crea")]
    public DateTime? FechCrea { get; set; }

    [Column("usua_modi")]
    public int? UsuaModi { get; set; }

    [Column("fech_modi")]
    public DateTime? FechModi { get; set; }
}