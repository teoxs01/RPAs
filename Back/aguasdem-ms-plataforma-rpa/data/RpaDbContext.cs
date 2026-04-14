namespace aguasdem_ms_plataforma_rpa.Data;

using Microsoft.EntityFrameworkCore;
using aguasdem_ms_plataforma_rpa.Models;

public class RpaDbContext : DbContext
{
    public RpaDbContext(DbContextOptions<RpaDbContext> options) : base(options)
    {
    }

    public DbSet<Automatizacion> Automatizaciones { get; set; }
    public DbSet<ConfiguracionProceso> ConfiguracionProcesos { get; set; }
    public DbSet<EjecucionRpa> Ejecuciones { get; set; }
}
