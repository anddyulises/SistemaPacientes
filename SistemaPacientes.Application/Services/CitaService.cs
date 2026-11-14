using System.Text.RegularExpressions;
using SistemaPacientes.Application.Contract;
using SistemaPacientes.Application.Core;
using SistemaPacientes.Application.Dtos;
using SistemaPacientes.Domain.Entities;
using SistemaPacientes.Infrastructure.Interfaces;

namespace SistemaPacientes.Application.Services;

public class CitaService : BaseService, ICitaService
{
    private readonly ICitaRepository _repo;
    private readonly IPacienteRepository _pacienteRepo;
    private readonly IMedicoRepository _medicoRepo;
    private readonly IConsultorioRepository _consultorioRepo;

    public CitaService(ICitaRepository repo, IPacienteRepository pacienteRepo, IMedicoRepository medicoRepo, IConsultorioRepository consultorioRepo)
    {
        _repo = repo;
        _pacienteRepo = pacienteRepo;
        _medicoRepo = medicoRepo;
        _consultorioRepo = consultorioRepo;
    }

    public Task<ServiceResult> GetAllAsync() => ExecuteAsync(async () =>
    {
        var list = await _repo.GetAllAsync();
        return Ok(list.Select(ToDto).ToList());
    });

    public Task<ServiceResult> GetByIdAsync(int id) => ExecuteAsync(async () =>
    {
        var entity = await _repo.GetByIdAsync(id);
        return entity is null ? NotFound("Cita") : Ok(ToDto(entity));
    });

    public Task<ServiceResult> AddAsync(AddCitaDto dto) => ExecuteAsync(async () =>
    {
        if (dto is null) return Fail("No se enviaron datos.");
        var errors = await ValidateAsync(dto, null);
        if (errors.Count > 0) return Fail(errors);

        var entity = new Cita();
        Apply(entity, dto);
        await _repo.AddAsync(entity);
        return Ok(ToDto(entity), "Registro creado correctamente.");
    });

    public Task<ServiceResult> UpdateAsync(UpdateCitaDto dto) => ExecuteAsync(async () =>
    {
        if (dto is null) return Fail("No se enviaron datos.");
        var entity = await _repo.GetByIdAsync(dto.Id);
        if (entity is null) return NotFound("Cita");

        var errors = await ValidateAsync(dto, dto.Id);
        if (errors.Count > 0) return Fail(errors);

        Apply(entity, dto);
        await _repo.UpdateAsync(entity);
        return Ok(ToDto(entity), "Registro actualizado correctamente.");
    });

    public Task<ServiceResult> RemoveAsync(int id) => ExecuteAsync(async () =>
    {
        if (!await _repo.ExistsAsync(id)) return NotFound("Cita");
        await _repo.DeleteAsync(id);
        return Ok(null, "Registro eliminado correctamente.");
    });

    // ---- Validaciones de cada campo ----
    private async Task<List<string>> ValidateAsync(AddCitaDto dto, int? id)
    {
        var errs = new List<string>();
        if (dto.PacienteId <= 0) errs.Add("Paciente: campo obligatorio.");
        else if (!await _pacienteRepo.ExistsAsync(dto.PacienteId)) errs.Add("Paciente: el registro seleccionado no existe.");
        if (dto.MedicoId <= 0) errs.Add("Médico: campo obligatorio.");
        else if (!await _medicoRepo.ExistsAsync(dto.MedicoId)) errs.Add("Médico: el registro seleccionado no existe.");
        if (dto.ConsultorioId <= 0) errs.Add("Consultorio: campo obligatorio.");
        else if (!await _consultorioRepo.ExistsAsync(dto.ConsultorioId)) errs.Add("Consultorio: el registro seleccionado no existe.");
        if (dto.FechaHora.Year < 2000) errs.Add("Fecha y hora: campo obligatorio y válido.");
        if (string.IsNullOrWhiteSpace(dto.Motivo)) errs.Add("Motivo: campo obligatorio.");
        else
        {
            if (dto.Motivo.Trim().Length > 300) errs.Add("Motivo: máximo 300 caracteres.");
        }
        if (string.IsNullOrWhiteSpace(dto.Estado) || !new[] { "Programada", "Completada", "Cancelada" }.Contains(dto.Estado)) errs.Add("Estado: valores permitidos: Programada, Completada, Cancelada.");
        return errs;
    }

    private static void Apply(Cita e, AddCitaDto dto)
    {
        e.PacienteId = dto.PacienteId;
        e.MedicoId = dto.MedicoId;
        e.ConsultorioId = dto.ConsultorioId;
        e.FechaHora = dto.FechaHora;
        e.Motivo = dto.Motivo.Trim();
        e.Estado = dto.Estado;
    }

    private static CitaDto ToDto(Cita e) => new()
    {
        Id = e.Id,
        CreationDate = e.CreationDate,
        PacienteId = e.PacienteId,
        MedicoId = e.MedicoId,
        ConsultorioId = e.ConsultorioId,
        FechaHora = e.FechaHora,
        Motivo = e.Motivo,
        Estado = e.Estado,
    };
}
