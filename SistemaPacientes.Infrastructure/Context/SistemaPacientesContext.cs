using Microsoft.EntityFrameworkCore;
using SistemaPacientes.Domain.Entities;

namespace SistemaPacientes.Infrastructure.Context;

public class SistemaPacientesContext : DbContext
{
    public SistemaPacientesContext(DbContextOptions<SistemaPacientesContext> options) : base(options) { }

    public DbSet<Especialidad> Especialidades { get; set; }
    public DbSet<Seguro> Seguros { get; set; }
    public DbSet<Paciente> Pacientes { get; set; }
    public DbSet<Medico> Medicos { get; set; }
    public DbSet<Consultorio> Consultorios { get; set; }
    public DbSet<Cita> Citas { get; set; }
    public DbSet<Diagnostico> Diagnosticos { get; set; }
    public DbSet<Consulta> Consultas { get; set; }
    public DbSet<Medicamento> Medicamentos { get; set; }
    public DbSet<Receta> Recetas { get; set; }
    public DbSet<RecetaDetalle> RecetaDetalles { get; set; }
    public DbSet<Alergia> Alergias { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Especialidad>(x =>
        {
            x.ToTable("Especialidades");
            x.Property(p => p.Nombre).IsRequired().HasMaxLength(100);
            x.Property(p => p.Descripcion).HasMaxLength(300);
        });
        modelBuilder.Entity<Seguro>(x =>
        {
            x.ToTable("Seguros");
            x.Property(p => p.Nombre).IsRequired().HasMaxLength(100);
            x.Property(p => p.Telefono).HasMaxLength(20);
            x.Property(p => p.Cobertura).HasPrecision(18, 2);
        });
        modelBuilder.Entity<Paciente>(x =>
        {
            x.ToTable("Pacientes");
            x.Property(p => p.Cedula).IsRequired().HasMaxLength(11);
            x.Property(p => p.Nombres).IsRequired().HasMaxLength(100);
            x.Property(p => p.Apellidos).IsRequired().HasMaxLength(100);
            x.Property(p => p.Sexo).IsRequired().HasMaxLength(20);
            x.Property(p => p.Telefono).HasMaxLength(20);
            x.Property(p => p.Email).HasMaxLength(150);
            x.Property(p => p.Direccion).HasMaxLength(200);
            x.HasOne<Seguro>().WithMany().HasForeignKey(p => p.SeguroId).IsRequired(false).OnDelete(DeleteBehavior.Restrict);
        });
        modelBuilder.Entity<Medico>(x =>
        {
            x.ToTable("Medicos");
            x.Property(p => p.Nombres).IsRequired().HasMaxLength(100);
            x.Property(p => p.Apellidos).IsRequired().HasMaxLength(100);
            x.Property(p => p.Exequatur).IsRequired().HasMaxLength(30);
            x.Property(p => p.Telefono).HasMaxLength(20);
            x.Property(p => p.Email).HasMaxLength(150);
            x.HasOne<Especialidad>().WithMany().HasForeignKey(p => p.EspecialidadId).IsRequired(true).OnDelete(DeleteBehavior.Restrict);
        });
        modelBuilder.Entity<Consultorio>(x =>
        {
            x.ToTable("Consultorios");
            x.Property(p => p.Codigo).IsRequired().HasMaxLength(20);
            x.Property(p => p.Ubicacion).HasMaxLength(150);
        });
        modelBuilder.Entity<Cita>(x =>
        {
            x.ToTable("Citas");
            x.HasOne<Paciente>().WithMany().HasForeignKey(p => p.PacienteId).IsRequired(true).OnDelete(DeleteBehavior.Restrict);
            x.HasOne<Medico>().WithMany().HasForeignKey(p => p.MedicoId).IsRequired(true).OnDelete(DeleteBehavior.Restrict);
            x.HasOne<Consultorio>().WithMany().HasForeignKey(p => p.ConsultorioId).IsRequired(true).OnDelete(DeleteBehavior.Restrict);
            x.Property(p => p.Motivo).IsRequired().HasMaxLength(300);
            x.Property(p => p.Estado).IsRequired().HasMaxLength(20);
        });
        modelBuilder.Entity<Diagnostico>(x =>
        {
            x.ToTable("Diagnosticos");
            x.Property(p => p.Nombre).IsRequired().HasMaxLength(150);
            x.Property(p => p.CodigoCie10).HasMaxLength(10);
            x.Property(p => p.Descripcion).HasMaxLength(300);
        });
        modelBuilder.Entity<Consulta>(x =>
        {
            x.ToTable("Consultas");
            x.HasOne<Cita>().WithMany().HasForeignKey(p => p.CitaId).IsRequired(true).OnDelete(DeleteBehavior.Restrict);
            x.HasOne<Diagnostico>().WithMany().HasForeignKey(p => p.DiagnosticoId).IsRequired(true).OnDelete(DeleteBehavior.Restrict);
            x.Property(p => p.Notas).HasMaxLength(500);
            x.Property(p => p.Costo).HasPrecision(18, 2);
        });
        modelBuilder.Entity<Medicamento>(x =>
        {
            x.ToTable("Medicamentos");
            x.Property(p => p.Nombre).IsRequired().HasMaxLength(150);
            x.Property(p => p.Presentacion).HasMaxLength(100);
        });
        modelBuilder.Entity<Receta>(x =>
        {
            x.ToTable("Recetas");
            x.HasOne<Consulta>().WithMany().HasForeignKey(p => p.ConsultaId).IsRequired(true).OnDelete(DeleteBehavior.Restrict);
            x.Property(p => p.Indicaciones).HasMaxLength(300);
        });
        modelBuilder.Entity<RecetaDetalle>(x =>
        {
            x.ToTable("RecetaDetalles");
            x.HasOne<Receta>().WithMany().HasForeignKey(p => p.RecetaId).IsRequired(true).OnDelete(DeleteBehavior.Restrict);
            x.HasOne<Medicamento>().WithMany().HasForeignKey(p => p.MedicamentoId).IsRequired(true).OnDelete(DeleteBehavior.Restrict);
            x.Property(p => p.Dosis).IsRequired().HasMaxLength(100);
        });
        modelBuilder.Entity<Alergia>(x =>
        {
            x.ToTable("Alergias");
            x.HasOne<Paciente>().WithMany().HasForeignKey(p => p.PacienteId).IsRequired(true).OnDelete(DeleteBehavior.Restrict);
            x.Property(p => p.Descripcion).IsRequired().HasMaxLength(200);
            x.Property(p => p.Severidad).IsRequired().HasMaxLength(20);
        });
    }
}
