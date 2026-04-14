namespace aguasdem_ms_plataforma_rpa.Controllers;

using Microsoft.AspNetCore.Mvc;
using aguasdem_ms_plataforma_rpa.Services;
using aguasdem_ms_plataforma_rpa.Models;

[ApiController]
[Route("api/rpa/automatizaciones")]
public class AutomationsController : ControllerBase
{
    private readonly AutomationService _service;
 
    public AutomationsController(AutomationService service)
    {
        _service = service;
    }
 
    [HttpGet]
    public async Task<IActionResult> Get()
        => Ok(await _service.GetAll());
 
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetById(id);
        return result == null ? NotFound() : Ok(result);
    }
 
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ConfiguracionProceso p)
        => Ok(await _service.Create(p));
 
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ConfiguracionProceso p)
    {
        var result = await _service.Update(id, p);
        return result == null ? NotFound() : Ok(result);
    }
 
    [HttpPatch("{id}/activo")]
    public async Task<IActionResult> SetActive(int id, [FromBody] ActivoRequest req)
    {
        var ok = await _service.SetActive(id, req.Activo);
        return ok ? Ok() : NotFound();
    }
 
    [HttpPost("{id}/ejecutar")]
    public async Task<IActionResult> Run(int id)
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
 
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _service.Delete(id);
        return ok ? Ok() : NotFound();
    }
}

public class ActivoRequest
{
    public bool Activo { get; set; }
}