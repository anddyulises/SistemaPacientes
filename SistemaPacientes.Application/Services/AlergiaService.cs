using System.Text.RegularExpressions;
using SistemaPacientes.Application.Contract;
using SistemaPacientes.Application.Core;
using SistemaPacientes.Application.Dtos;
using SistemaPacientes.Domain.Entities;
using SistemaPacientes.Infrastructure.Interfaces;

namespace SistemaPacientes.Application.Services;

public class AlergiaService : BaseService, IAlergiaService
{
    private readonly IAlergiaRepository _repo;
    private readonly IPacienteRepository _pacienteRepo;

    public AlergiaService(IAlergiaRepository repo, IPacienteRepository pacienteRepo)
    {
        _repo = repo;
        _pacienteRepo = pacienteRepo;
    }

    public Task<ServiceResult> GetAllAsync() => ExecuteAsync(async () =>
    {
        var list = await _repo.GetAllAsync();
        return Ok(list.Select(ToDto).ToList());
    });

    public Task<ServiceResult> GetByIdAsync(int id) => ExecuteAsync(async () =>
    {
        var entity = await _repo.GetByIdAsync(id);
        return entity is null ? NotFound("Alergia") : Ok(ToDto(entity));
    });

    public Task<ServiceResult> AddAsync(AddAlergiaDto dto) => ExecuteAsync(async () =>
    {
        if (dto is null) return Fail("No se enviaron datos.");
        var errors = await ValidateAsync(dto, null);
        if (errors.Count > 0) return Fail(errors);

        var entity = new Alergia();
        Apply(entity, dto);
        await _repo.AddAsync(entity);
        return Ok(ToDto(entity), "Registro creado correctamente.");
    });

    public Task<ServiceResult> UpdateAsync(UpdateAlergiaDto dto) => ExecuteAsync(async () =>
    {
        if (dto is null) return Fail("No se enviaron datos.");
        var entity = await _repo.GetByIdAsync(dto.Id);
        if (entity is null) return NotFound("Alergia");

        var errors = await ValidateAsync(dto, dto.Id);
        if (errors.Count > 0) return Fail(errors);

        Apply(entity, dto);
        await _repo.UpdateAsync(entity);
        return Ok(ToDto(entity), "Registro actualizado correctamente.");
    });

    public Task<ServiceResult> RemoveAsync(int id) => ExecuteAsync(async () =>
    {
        if (!await _repo.ExistsAsync(id)) return NotFound("Alergia");
        await _repo.DeleteAsync(id);
        return Ok(null, "Registro eliminado correctamente.");
    });

    // ---- Validaciones de cada campo ----
    private async Task<List<string>> ValidateAsync(AddAlergiaDto dto, int? id)
    {
        var errs = new List<string>();
        if (dto.PacienteId <= 0) errs.Add("Paciente: campo obligatorio.");
        else if (!await _pacienteRepo.ExistsAsync(dto.PacienteId)) errs.Add("Paciente: el registro seleccionado no existe.");
        if (string.IsNullOrWhiteSpace(dto.Descripcion)) errs.Add("Descripción: campo obligatorio.");
        else
        {
            if (dto.Descripcion.Trim().Length > 200) errs.Add("Descripción: máximo 200 caracteres.");
        }
        if (string.IsNullOrWhiteSpace(dto.Severidad) || !new[] { "Leve", "Moderada", "Grave" }.Contains(dto.Severidad)) errs.Add("Severidad: valores permitidos: Leve, Moderada, Grave.");
        return errs;
    }

    private static void Apply(Alergia e, AddAlergiaDto dto)
    {
        e.PacienteId = dto.PacienteId;
        e.Descripcion = dto.Descripcion.Trim();
        e.Severidad = dto.Severidad;
    }

    private static AlergiaDto ToDto(Alergia e) => new()
    {
        Id = e.Id,
        CreationDate = e.CreationDate,
        PacienteId = e.PacienteId,
        Descripcion = e.Descripcion,
        Severidad = e.Severidad,
    };
}
