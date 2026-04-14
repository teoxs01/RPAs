using Microsoft.EntityFrameworkCore;
using aguasdem_ms_plataforma_rpa.Data;
using aguasdem_ms_plataforma_rpa.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. CORS debe estar antes de todo (Services)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader()
              .SetPreflightMaxAge(TimeSpan.FromMinutes(10));
    });
});

// =============================
// 🔌 CONEXIÓN A POSTGRESQL
// =============================
builder.Services.AddDbContext<RpaDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));


// =============================
// 📦 SERVICIOS (LÓGICA)
// =============================
builder.Services.AddScoped<AutomationService>();
builder.Services.AddScoped<ExecutionService>();


// =============================
// 🎮 CONTROLLERS
// =============================
builder.Services.AddControllers();


// =============================
// 📄 SWAGGER (opcional pero PRO)
// =============================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// =============================
// 🚀 BUILD APP
// =============================
var app = builder.Build();

// 2. Aplicar CORS inmediatamente (punto más alto del pipeline)
app.UseCors("AllowAll");

// Manejador global de errores
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        // Re-aplicar CORS en caso de error para que el navegador vea la respuesta
        context.Response.Headers.AccessControlAllowOrigin = "*";
        context.Response.Headers.AccessControlAllowHeaders = "*";
        context.Response.Headers.AccessControlAllowMethods = "*";
        
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";
        
        var error = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();
        if (error != null)
        {
            var result = System.Text.Json.JsonSerializer.Serialize(new { 
                message = "Error interno del servidor", 
                detail = error.Error.Message 
            });
            await context.Response.WriteAsync(result);
        }
    });
});


// =============================
// 🧪 SWAGGER
// =============================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// =============================
// 🎯 MAPEAR CONTROLLERS
// =============================
app.MapControllers();


// =============================
// ▶️ RUN
// =============================
app.Run();