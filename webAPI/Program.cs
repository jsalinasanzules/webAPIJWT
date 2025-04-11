using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Text.Json.Serialization;
using webAPI.Helpers;
using webAPI.Models;

//servicio para coneccion a BD
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<DbUsuarioContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("connectionDB"))
    );

// Add services to the container.
builder.Services.AddCors(); //usar los cors para permitir cookies en direcciones

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();//servicio swagger
builder.Services.AddScoped<JwtService>();//servicio para JWT

//quitar el ciclo de JSON
/*builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);*/

//configuracion de serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();
var app = builder.Build();

//redireccionamiento para swagger
app.MapGet("/", (HttpContext context) =>
{
    context.Response.Redirect("/swagger/index.html");
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI();
    app.UseSwagger();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

//configuracion de rutas cors
app.UseCors(options => options
    .WithOrigins(new[] {"http://localhost:3000", "http://localhost:44316", "http://localhost:8080", "http://localhost:4200" })
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
);

app.UseAuthorization();

app.MapControllers();
Log.Information("Aplicacion ejecutada");

app.Run();
