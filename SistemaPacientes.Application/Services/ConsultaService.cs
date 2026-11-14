using System.Text.RegularExpressions;
using SistemaPacientes.Application.Contract;
using SistemaPacientes.Application.Core;
using SistemaPacientes.Application.Dtos;
using SistemaPacientes.Domain.Entities;
using SistemaPacientes.Infrastructure.Interfaces;

namespace SistemaPacientes.Application.Services;

public class ConsultaService : BaseService, IConsultaService
{
    private readonly IConsultaRepository _repo;
    private readonly ICitaRepository _citaRepo;
    private readonly IDiagnosticoRepository _diagnosticoRepo;

    public ConsultaService(IConsultaRepository repo, ICitaRepository citaRepo, IDiagnosticoRepository diagnosticoRepo)
    {
        _repo = repo;
        _citaRepo = citaRepo;
        _diagnosticoRepo = diagnosticoRepo;
    }

    public Task<ServiceResult> GetAllAsync() => ExecuteAsync(async () =>
    {
        var list = await _repo.GetAllAsync();
        return Ok(list.Select(ToDto).ToList());
    });

    public Task<ServiceResult> GetByIdAsync(int id) => ExecuteAsync(async () =>
    {
        var entity = await _repo.GetByIdAsync(id);
        return entity is null ? NotFound("Consulta") : Ok(ToDto(entity));
    });

    public Task<ServiceResult> AddAsync(AddConsultaDto dto) => ExecuteAsync(async () =>
    {
        if (dto is null) return Fail("No se enviaron datos.");
        var errors = await ValidateAsync(dto, null);
        if (errors.Count > 0) return Fail(errors);

        var entity = new Consulta();
        Apply(entity, dto);
        await _repo.AddAsync(entity);
        return Ok(ToDto(entity), "Registro creado correctamente.");
    });

    public Task<ServiceResult> UpdateAsync(UpdateConsultaDto dto) => ExecuteAsync(async () =>
    {
        if (dto is null) return Fail("No se enviaron datos.");
        var entity = await _repo.GetByIdAsync(dto.Id);
        if (entity is null) return NotFound("Consulta");

        var errors = await ValidateAsync(dto, dto.Id);
        if (errors.Count > 0) return Fail(errors);

        Apply(entity, dto);
        await _repo.UpdateAsync(entity);
        return Ok(ToDto(entity), "Registro actualizado correctamente.");
    });

    public Task<ServiceResult> RemoveAsync(int id) => ExecuteAsync(async () =>
    {
        if (!await _repo.ExistsAsync(id)) return NotFound("Consulta");
        await _repo.DeleteAsync(id);
        return Ok(null, "Registro eliminado correctamente.");
    });

    // ---- Validaciones de cada campo ----
    private async Task<List<string>> ValidateAsync(AddConsultaDto dto, int? id)
    {
        var errs = new List<string>();
        if (dto.CitaId <= 0) errs.Add("Cita: campo obligatorio.");
        else if (!await _citaRepo.ExistsAsync(dto.CitaId)) errs.Add("Cita: el registro seleccionado no existe.");
        if (dto.DiagnosticoId <= 0) errs.Add("Diagnóstico: campo obligatorio.");
        else if (!await _diagnosticoRepo.ExistsAsync(dto.DiagnosticoId)) errs.Add("Diagnóstico: el registro seleccionado no existe.");
        if (!string.IsNullOrWhiteSpace(dto.Notas))
        {
            if (dto.Notas.Trim().Length > 500) errs.Add("Notas: máximo 500 caracteres.");
        }
        if (dto.Costo < 0m || dto.Costo > 999999999.99m) errs.Add("Costo (RD$): debe estar entre 0 y 999999999.99.");
        return errs;
    }

    private static void Apply(Consulta e, AddConsultaDto dto)
    {
        e.CitaId = dto.CitaId;
        e.DiagnosticoId = dto.DiagnosticoId;
        e.Notas = string.IsNullOrWhiteSpace(dto.Notas) ? null : dto.Notas.Trim();
        e.Costo = dto.Costo;
    }

    private static ConsultaDto ToDto(Consulta e) => new()
    {
        Id = e.Id,
        CreationDate = e.CreationDate,
        CitaId = e.CitaId,
        DiagnosticoId = e.DiagnosticoId,
        Notas = e.Notas,
        Costo = e.Costo,
    };
}
