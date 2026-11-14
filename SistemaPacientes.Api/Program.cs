using Microsoft.EntityFrameworkCore;
using SistemaPacientes.Application.Contract;
using SistemaPacientes.Application.Services;
using SistemaPacientes.Infrastructure.Context;
using SistemaPacientes.Infrastructure.Interfaces;
using SistemaPacientes.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    // Las validaciones las hacen los servicios (capa de aplicación) y devuelven un ServiceResult.
    .ConfigureApiBehaviorOptions(o => o.SuppressModelStateInvalidFilter = true);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS: permite que el proyecto web (frontend) consuma el API.
builder.Services.AddCors(o => o.AddPolicy("Frontend", p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

builder.Services.AddDbContext<SistemaPacientesContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositorios (Infrastructure)
builder.Services.AddScoped<IEspecialidadRepository, EspecialidadRepository>();
builder.Services.AddScoped<ISeguroRepository, SeguroRepository>();
builder.Services.AddScoped<IPacienteRepository, PacienteRepository>();
builder.Services.AddScoped<IMedicoRepository, MedicoRepository>();
builder.Services.AddScoped<IConsultorioRepository, ConsultorioRepository>();
builder.Services.AddScoped<ICitaRepository, CitaRepository>();
builder.Services.AddScoped<IDiagnosticoRepository, DiagnosticoRepository>();
builder.Services.AddScoped<IConsultaRepository, ConsultaRepository>();
builder.Services.AddScoped<IMedicamentoRepository, MedicamentoRepository>();
builder.Services.AddScoped<IRecetaRepository, RecetaRepository>();
builder.Services.AddScoped<IRecetaDetalleRepository, RecetaDetalleRepository>();
builder.Services.AddScoped<IAlergiaRepository, AlergiaRepository>();

// Servicios (Application)
builder.Services.AddScoped<IEspecialidadService, EspecialidadService>();
builder.Services.AddScoped<ISeguroService, SeguroService>();
builder.Services.AddScoped<IPacienteService, PacienteService>();
builder.Services.AddScoped<IMedicoService, MedicoService>();
builder.Services.AddScoped<IConsultorioService, ConsultorioService>();
builder.Services.AddScoped<ICitaService, CitaService>();
builder.Services.AddScoped<IDiagnosticoService, DiagnosticoService>();
builder.Services.AddScoped<IConsultaService, ConsultaService>();
builder.Services.AddScoped<IMedicamentoService, MedicamentoService>();
builder.Services.AddScoped<IRecetaService, RecetaService>();
builder.Services.AddScoped<IRecetaDetalleService, RecetaDetalleService>();
builder.Services.AddScoped<IAlergiaService, AlergiaService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SistemaPacientesContext>();
    db.Database.EnsureCreated();
    DataSeeder.Seed(db);
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("Frontend");
app.UseAuthorization();
app.MapControllers();
app.Run();
