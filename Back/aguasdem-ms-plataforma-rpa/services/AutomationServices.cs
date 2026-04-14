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
                .Where(e => e.AutoId == auto.Id)
                .OrderByDescending(e => e.FechaInicio)
                .FirstOrDefaultAsync();

            results.Add(new
            {
                id = auto.Id,
                nombre = auto.Nombre,
                scriptPath = auto.ScriptPath,
                horaEjecucion = auto.HoraEjecucion.ToString(@"hh\:mm"),
                diasEjecucion = auto.DiasSemana,
                activo = auto.Estado,
                ultimoEstado = lastExecution?.Estado ?? "Pendiente",
                ultimaEjecucion = lastExecution?.FechaInicio // Enviamos el objeto DateTime directamente
            });
        }

        return results;
    }
 
    public async Task<ConfiguracionProceso?> GetById(int id)
        => await _context.ConfiguracionProcesos.FindAsync(id);

    public async Task<ConfiguracionProceso> Create(ConfiguracionProceso p)
    {
        p.UsuaCrea = 1; // Por ahora valor por defecto
        p.FechCrea = DateTime.UtcNow;
        _context.ConfiguracionProcesos.Add(p);
        await _context.SaveChangesAsync();
        return p;
    }

    public async Task<ConfiguracionProceso?> Update(int id, ConfiguracionProceso p)
    {
        var existing = await _context.ConfiguracionProcesos.FindAsync(id);
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

    public async Task<bool> SetActive(int id, bool activo)
    {
        var existing = await _context.ConfiguracionProcesos.FindAsync(id);
        if (existing == null) return false;

        existing.Estado = activo;
        existing.UsuaModi = 1; // Por ahora valor por defecto
        existing.FechModi = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Delete(int id)
    {
        var existing = await _context.ConfiguracionProcesos.FindAsync(id);
        if (existing == null) return false;

        _context.ConfiguracionProcesos.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }
 
    public async Task Run(int id)
    {
        var proceso = await _context.ConfiguracionProcesos.FindAsync(id);
 
        if (proceso == null)
            throw new Exception("No encontrado");
 
        await _execution.EjecutarProceso(proceso);
    }
}