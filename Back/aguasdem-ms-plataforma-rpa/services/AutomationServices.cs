namespace aguasdem_ms_plataforma_rpa.Services;

using System.Linq;
using Microsoft.EntityFrameworkCore;
using aguasdem_ms_plataforma_rpa.Data;
using aguasdem_ms_plataforma_rpa.Models;

public class AutomationService
{
    private readonly RpaDbContext _context;
    private readonly ExecutionService _execution;
 
    public AutomationService(RpaDbContext context, ExecutionService execution)
    {
        _context = context;
        _execution = execution;
    }
 
    public async Task<object> GetAll()
    {
        var configs = await _context.ConfiguracionProcesos
            .Include(c => c.Automatizacion)
            .ThenInclude(a => a.AppConfig)
            .ToListAsync();
            
        var results = new List<object>();

        foreach (var config in configs)
        {
            var lastExecution = await _context.Ejecuciones
                .Where(e => e.AutoId == config.AutoId)
                .OrderByDescending(e => e.FechaInicio)
                .FirstOrDefaultAsync();

            results.Add(new
            {
                autoId = config.AutoId,
                codigo = config.Automatizacion?.Codigo ?? string.Empty,
                nombre = config.Nombre,
                descripcion = config.Automatizacion?.Descripcion,
                tipo = config.Automatizacion?.Tipo ?? string.Empty,
                entorno = config.Automatizacion?.Entorno ?? string.Empty,
                scriptPath = config.ScriptPath,
                horaEjecucion = config.HoraEjecucion.ToString(@"hh\:mm"),
                diasEjecucion = config.DiasSemana,
                activo = config.Estado,
                ultimoEstado = lastExecution?.Estado ?? "Pendiente",
                ultimaEjecucion = lastExecution?.FechaInicio,
                // Nuevos campos
                appName = config.Automatizacion?.AppConfig?.AppName,
                appUser = config.Automatizacion?.AppConfig?.User,
                appPassword = config.Automatizacion?.AppConfig?.Password,
                appUrl = config.Automatizacion?.AppConfig?.Url
            });
        }

        return results;
    }
 
    public async Task<ConfiguracionProceso?> GetById(long autoId)
        => await _context.ConfiguracionProcesos.FirstOrDefaultAsync(c => c.AutoId == autoId);

    public async Task<ConfiguracionProceso> Create(ConfiguracionProceso p)
    {
        p.UsuaCrea = 1; // Por ahora valor por defecto
        p.FechCrea = DateTime.UtcNow;
        _context.ConfiguracionProcesos.Add(p);
        await _context.SaveChangesAsync();
        return p;
    }

    public async Task<ConfiguracionProceso?> Update(long autoId, ConfiguracionProceso p)
    {
        var existing = await _context.ConfiguracionProcesos.FirstOrDefaultAsync(c => c.AutoId == autoId);
        if (existing == null) return null;

        existing.Nombre = p.Nombre;
        existing.ScriptPath = p.ScriptPath;
        existing.HoraEjecucion = p.HoraEjecucion;
        existing.DiasSemana = p.DiasSemana;
        existing.Estado = p.Estado;
        existing.UsuaModi = 1; // Por ahora valor por defecto
        existing.FechModi = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> SetActive(long autoId, bool activo)
    {
        var existing = await _context.ConfiguracionProcesos.FirstOrDefaultAsync(c => c.AutoId == autoId);
        if (existing == null) return false;

        existing.Estado = activo;
        existing.UsuaModi = 1; // Por ahora valor por defecto
        existing.FechModi = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Delete(long autoId)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // 1. Eliminar AppConfiguration
            var appConfig = await _context.AppConfigurations.FirstOrDefaultAsync(ac => ac.AutoId == autoId);
            if (appConfig != null) _context.AppConfigurations.Remove(appConfig);

            // 2. Eliminar ConfiguracionProceso
            var config = await _context.ConfiguracionProcesos.FirstOrDefaultAsync(c => c.AutoId == autoId);
            if (config != null) _context.ConfiguracionProcesos.Remove(config);

            // 3. Eliminar Automatizacion (Maestro)
            var auto = await _context.Automatizaciones.FirstOrDefaultAsync(a => a.AutoId == autoId);
            if (auto != null) _context.Automatizaciones.Remove(auto);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            return false;
        }
    }
 
    public async Task Run(long autoId)
    {
        var proceso = await _context.ConfiguracionProcesos.FirstOrDefaultAsync(c => c.AutoId == autoId);
 
        if (proceso == null)
            throw new Exception("No encontrado");
 
        await _execution.EjecutarProceso(proceso);
    }
}