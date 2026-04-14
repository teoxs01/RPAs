namespace aguasdem_ms_plataforma_rpa.Services;

using System.Diagnostics;
using aguasdem_ms_plataforma_rpa.Data;
using aguasdem_ms_plataforma_rpa.Models;

public class ExecutionService
{
    private readonly RpaDbContext _context;
 
    public ExecutionService(RpaDbContext context)
    {
        _context = context;
    }
 
    public async Task EjecutarProceso(ConfiguracionProceso proceso)
    {
        var ejec = new EjecucionRpa
        {
            AutoId = proceso.Id,
            Estado = "EN_PROCESO",
            FechaInicio = DateTime.UtcNow,
            UsuaCrea = 1, // Por ahora valor por defecto
            Servidor = Environment.MachineName,
            Ambiente = "DESARROLLO"
        };
 
        _context.Ejecuciones.Add(ejec);
        await _context.SaveChangesAsync();
 
        try
        {
            var start = DateTime.UtcNow;
 
            var proc = Process.Start(new ProcessStartInfo
            {
                FileName = proceso.ScriptPath,
                UseShellExecute = false
            });
 
            if (proc != null)
            {
                await proc.WaitForExitAsync();
            }
 
            var end = DateTime.UtcNow;
 
            ejec.Estado = "OK";
            ejec.FechaFin = end;
            ejec.DuracionMs = (long)(end - start).TotalMilliseconds;
        }
        catch (Exception ex)
        {
            ejec.Estado = "ERROR";
            ejec.MensajeError = ex.Message;
            ejec.FechaFin = DateTime.UtcNow;
        }
 
        await _context.SaveChangesAsync();
    }
}