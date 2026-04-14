using Microsoft.EntityFrameworkCore;
using aguasdem_ms_plataforma_rpa.Data;
using aguasdem_ms_plataforma_rpa.Services;

var builder = WebApplication.CreateBuilder(args);


// =============================
// 🔌 CONEXIÓN A POSTGRESQL
// =============================
builder.Services.AddDbContext<RpaDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));


// =============================
// 🌐 CORS (para Vue)
// =============================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.SetIsOriginAllowed(_ => true) // Más robusto que AllowAnyOrigin para algunos navegadores
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials(); // Permitir credenciales si fuera necesario
        });
});


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

// Manejador global de errores con CORS forzado
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.Headers["Access-Control-Allow-Origin"] = "*";
        context.Response.Headers["Access-Control-Allow-Methods"] = "*";
        context.Response.Headers["Access-Control-Allow-Headers"] = "*";
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

// Middleware manual para OPTIONS y CORS general
app.Use(async (context, next) =>
{
    context.Response.Headers["Access-Control-Allow-Origin"] = "*";
    context.Response.Headers["Access-Control-Allow-Methods"] = "*";
    context.Response.Headers["Access-Control-Allow-Headers"] = "*";
    if (context.Request.Method == "OPTIONS")
    {
        context.Response.StatusCode = 200;
        return;
    }
    await next();
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
// 🌐 CORS (activar)
// =============================
app.UseCors("AllowAll");


// =============================
// 🔐 (opcional futuro)
// app.UseAuthentication();
// app.UseAuthorization();
// =============================


// =============================
// 🎯 MAPEAR CONTROLLERS
// =============================
app.MapControllers();


// =============================
// ▶️ RUN
// =============================
app.Run();