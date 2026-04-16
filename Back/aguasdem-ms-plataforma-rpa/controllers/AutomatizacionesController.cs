namespace aguasdem_ms_plataforma_rpa.Controllers;

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using aguasdem_ms_plataforma_rpa.Data;
using aguasdem_ms_plataforma_rpa.Models;
using aguasdem_ms_plataforma_rpa.Services;

[ApiController]
[Route("api/rpa/automatizaciones")]
public class AutomatizacionesController : ControllerBase
{
    private readonly RpaDbContext _context;
    private readonly AutomationService _service;

    public AutomatizacionesController(RpaDbContext context, AutomationService service)
    {
        _context = context;
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        // Usamos el servicio para obtener la vista consolidada que espera el dashboard
        return Ok(await _service.GetAll());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        var result = await _service.GetById(id);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AutomatizacionCreateRequest request)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var automatizacion = new Automatizacion
            {
                Codigo = request.Codigo,
                Nombre = request.Nombre,
                Descripcion = request.Descripcion,
                Tipo = request.Tipo,
                Entorno = request.Entorno ?? "PROD",
                UsuaCrea = 1,
                FechCrea = DateTime.UtcNow
            };

            _context.Automatizaciones.Add(automatizacion);
            await _context.SaveChangesAsync();

            TimeSpan.TryParse(request.HoraEjecucion, out var horaEjecucion);

            var config = new ConfiguracionProceso
            {
                AutoId = automatizacion.AutoId,
                Nombre = automatizacion.Nombre,
                ScriptPath = request.ScriptPath ?? string.Empty,
                HoraEjecucion = horaEjecucion,
                DiasSemana = request.DiasSemana,
                Estado = request.Activo,
                UsuaCrea = 1,
                FechCrea = DateTime.UtcNow
            };

            _context.ConfiguracionProcesos.Add(config);
            await _context.SaveChangesAsync();

            // Crear AppConfiguration si se proporcionaron datos
            if (!string.IsNullOrEmpty(request.AppName))
            {
                var appConfig = new AppConfiguration
                {
                    AutoId = automatizacion.AutoId,
                    AppName = request.AppName,
                    User = request.AppUser ?? string.Empty,
                    Password = request.AppPassword ?? string.Empty,
                    Url = request.AppUrl,
                    Status = "ACTIVO",
                    Date = DateTime.UtcNow
                };
                _context.AppConfigurations.Add(appConfig);
                await _context.SaveChangesAsync();
            }

            await transaction.CommitAsync();
            return Ok(automatizacion);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return BadRequest(new { message = "Error al crear la automatización y su configuración", detail = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, [FromBody] AutomatizacionCreateRequest request)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var existing = await _context.Automatizaciones.FindAsync(id);
            if (existing == null) return NotFound();

            existing.Codigo = request.Codigo;
            existing.Nombre = request.Nombre;
            existing.Descripcion = request.Descripcion;
            existing.Tipo = request.Tipo;
            existing.Entorno = request.Entorno ?? "PROD";
            existing.UsuaModi = 1;
            existing.FechModi = DateTime.UtcNow;

            // Actualizar o crear la configuración asociada
            var config = await _context.ConfiguracionProcesos
                .FirstOrDefaultAsync(c => c.AutoId == id);

            if (config == null)
            {
                config = new ConfiguracionProceso { AutoId = id, UsuaCrea = 1, FechCrea = DateTime.UtcNow };
                _context.ConfiguracionProcesos.Add(config);
            }

            config.Nombre = existing.Nombre;
            config.ScriptPath = request.ScriptPath ?? string.Empty;
            TimeSpan.TryParse(request.HoraEjecucion, out var horaEjecucion);
            config.HoraEjecucion = horaEjecucion;
            config.DiasSemana = request.DiasSemana;
            config.Estado = request.Activo;
            config.UsuaModi = 1;
            config.FechModi = DateTime.UtcNow;

            // Actualizar o crear AppConfiguration si se proporcionaron datos
            if (!string.IsNullOrEmpty(request.AppName))
            {
                var appConfig = await _context.AppConfigurations
                    .FirstOrDefaultAsync(ac => ac.AutoId == id);

                if (appConfig == null)
                {
                    appConfig = new AppConfiguration { AutoId = id, Status = "ACTIVO", Date = DateTime.UtcNow };
                    _context.AppConfigurations.Add(appConfig);
                }

                appConfig.AppName = request.AppName;
                appConfig.User = request.AppUser ?? string.Empty;
                appConfig.Password = request.AppPassword ?? string.Empty;
                appConfig.Url = request.AppUrl;
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return Ok(existing);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return BadRequest(new { message = "Error al actualizar la automatización y su configuración", detail = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var ok = await _service.Delete(id);
        return ok ? Ok() : NotFound();
    }

    [HttpPatch("{id}/activo")]
    public async Task<IActionResult> SetActive(long id, [FromBody] ActivoRequest req)
    {
        var ok = await _service.SetActive(id, req.Activo);
        return ok ? Ok() : NotFound();
    }

    [HttpPost("{id}/ejecutar")]
    public async Task<IActionResult> Run(long id)
    {
        try 
        {
            await _service.Run(id);
            return Ok(new { message = "Ejecutado correctamente" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("{id}/configuracion")]
    public async Task<IActionResult> SaveConfig(long id, [FromBody] ConfiguracionProcesoRequest request)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var automatizacion = await _context.Automatizaciones.FindAsync(id);
            if (automatizacion == null) return NotFound("La automatización no existe");

            var config = await _context.ConfiguracionProcesos
                .FirstOrDefaultAsync(c => c.AutoId == id);

            if (config == null)
            {
                config = new ConfiguracionProceso { AutoId = id };
                _context.ConfiguracionProcesos.Add(config);
            }

            config.Nombre = automatizacion.Nombre; // Sincronizamos el nombre
            config.ScriptPath = request.ScriptPath;
            TimeSpan.TryParse(request.HoraEjecucion, out var hora);
            config.HoraEjecucion = hora;
            config.DiasSemana = request.DiasSemana;
            config.Estado = request.Activo;
            config.UsuaModi = 1;
            config.FechModi = DateTime.UtcNow;

            // También actualizamos AppConfiguration si viene en el request
            if (!string.IsNullOrEmpty(request.AppName))
            {
                var appConfig = await _context.AppConfigurations
                    .FirstOrDefaultAsync(ac => ac.AutoId == id);

                if (appConfig == null)
                {
                    appConfig = new AppConfiguration { AutoId = id, Status = "ACTIVO", Date = DateTime.UtcNow };
                    _context.AppConfigurations.Add(appConfig);
                }

                appConfig.AppName = request.AppName;
                appConfig.User = request.AppUser ?? string.Empty;
                appConfig.Password = request.AppPassword ?? string.Empty;
                appConfig.Url = request.AppUrl;
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return Ok(config);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return BadRequest(new { message = "Error al guardar la configuración", detail = ex.Message });
        }
    }
}

public class ActivoRequest
{
    [JsonPropertyName("activo")]
    public bool Activo { get; set; }
}

public class AutomatizacionCreateRequest
{
    [JsonPropertyName("codigo")]
    public string Codigo { get; set; } = string.Empty;
    
    [JsonPropertyName("nombre")]
    public string Nombre { get; set; } = string.Empty;
    
    [JsonPropertyName("descripcion")]
    public string? Descripcion { get; set; }
    
    [JsonPropertyName("tipo")]
    public string Tipo { get; set; } = string.Empty;
    
    [JsonPropertyName("entorno")]
    public string? Entorno { get; set; }
    
    [JsonPropertyName("scriptPath")]
    public string? ScriptPath { get; set; }
    
    [JsonPropertyName("horaEjecucion")]
    public string HoraEjecucion { get; set; } = "08:00";
    
    [JsonPropertyName("diasEjecucion")]
    public List<string> DiasSemana { get; set; } = new();
    
    [JsonPropertyName("activo")]
    public bool Activo { get; set; }

    // Campos de AppConfiguration (opcionales)
    [JsonPropertyName("appName")]
    public string? AppName { get; set; }
    
    [JsonPropertyName("appUser")]
    public string? AppUser { get; set; }
    
    [JsonPropertyName("appPassword")]
    public string? AppPassword { get; set; }
    
    [JsonPropertyName("appUrl")]
    public string? AppUrl { get; set; }
}

public class ConfiguracionProcesoRequest
{
    [JsonPropertyName("scriptPath")]
    public string ScriptPath { get; set; } = string.Empty;
    
    [JsonPropertyName("horaEjecucion")]
    public string HoraEjecucion { get; set; } = "08:00";
    
    [JsonPropertyName("diasSemana")]
    public List<string> DiasSemana { get; set; } = new();
    
    [JsonPropertyName("activo")]
    public bool Activo { get; set; }

    // Campos de AppConfiguration (opcionales)
    [JsonPropertyName("appName")]
    public string? AppName { get; set; }
    
    [JsonPropertyName("appUser")]
    public string? AppUser { get; set; }
    
    [JsonPropertyName("appPassword")]
    public string? AppPassword { get; set; }
    
    [JsonPropertyName("appUrl")]
    public string? AppUrl { get; set; }
}
