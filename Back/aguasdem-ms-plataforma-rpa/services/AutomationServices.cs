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
        var automations = await _context.ConfiguracionProcesos.ToListAsync();
        var results = new List<object>();

        foreach (var auto in automations)
        {
            var lastExecution = await _context.Ejecuciones
                .Where(e => e.AutoId == auto.AutoId)
                .OrderByDescending(e => e.FechaInicio)
                .FirstOrDefaultAsync();

            results.Add(new
            {
                autoId = auto.AutoId,
                nombre = auto.Nombre,
                scriptPath = auto.ScriptPath,
                horaEjecucion = auto.HoraEjecucion.ToString(@"hh\:mm"),
                diasEjecucion = auto.DiasSemana,
                activo = auto.Estado,
                ultimoEstado = lastExecution?.Estado ?? "Pendiente",
                ultimaEjecucion = lastExecution?.FechaInicio
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
        var existing = await _context.ConfiguracionProcesos.FirstOrDefaultAsync(c => c.AutoId == autoId);
        if (existing == null) return false;

        _context.ConfiguracionProcesos.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }
 
    public async Task Run(long autoId)
    {
        var proceso = await _context.ConfiguracionProcesos.FirstOrDefaultAsync(c => c.AutoId == autoId);
 
        if (proceso == null)
            throw new Exception("No encontrado");
 
        await _execution.EjecutarProceso(proceso);
    }
}