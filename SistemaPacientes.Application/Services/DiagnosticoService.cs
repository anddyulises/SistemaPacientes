using System.Text.RegularExpressions;
using SistemaPacientes.Application.Contract;
using SistemaPacientes.Application.Core;
using SistemaPacientes.Application.Dtos;
using SistemaPacientes.Domain.Entities;
using SistemaPacientes.Infrastructure.Interfaces;

namespace SistemaPacientes.Application.Services;

public class DiagnosticoService : BaseService, IDiagnosticoService
{
    private readonly IDiagnosticoRepository _repo;

    public DiagnosticoService(IDiagnosticoRepository repo)
    {
        _repo = repo;
    }

    public Task<ServiceResult> GetAllAsync() => ExecuteAsync(async () =>
    {
        var list = await _repo.GetAllAsync();
        return Ok(list.Select(ToDto).ToList());
    });

    public Task<ServiceResult> GetByIdAsync(int id) => ExecuteAsync(async () =>
    {
        var entity = await _repo.GetByIdAsync(id);
        return entity is null ? NotFound("Diagnostico") : Ok(ToDto(entity));
    });

    public Task<ServiceResult> AddAsync(AddDiagnosticoDto dto) => ExecuteAsync(async () =>
    {
        if (dto is null) return Fail("No se enviaron datos.");
        var errors = await ValidateAsync(dto, null);
        if (errors.Count > 0) return Fail(errors);

        var entity = new Diagnostico();
        Apply(entity, dto);
        await _repo.AddAsync(entity);
        return Ok(ToDto(entity), "Registro creado correctamente.");
    });

    public Task<ServiceResult> UpdateAsync(UpdateDiagnosticoDto dto) => ExecuteAsync(async () =>
    {
        if (dto is null) return Fail("No se enviaron datos.");
        var entity = await _repo.GetByIdAsync(dto.Id);
        if (entity is null) return NotFound("Diagnostico");

        var errors = await ValidateAsync(dto, dto.Id);
        if (errors.Count > 0) return Fail(errors);

        Apply(entity, dto);
        await _repo.UpdateAsync(entity);
        return Ok(ToDto(entity), "Registro actualizado correctamente.");
    });

    public Task<ServiceResult> RemoveAsync(int id) => ExecuteAsync(async () =>
    {
        if (!await _repo.ExistsAsync(id)) return NotFound("Diagnostico");
        await _repo.DeleteAsync(id);
        return Ok(null, "Registro eliminado correctamente.");
    });

    // ---- Validaciones de cada campo ----
    private async Task<List<string>> ValidateAsync(AddDiagnosticoDto dto, int? id)
    {
        var errs = new List<string>();
        if (string.IsNullOrWhiteSpace(dto.Nombre)) errs.Add("Nombre: campo obligatorio.");
        else
        {
            if (dto.Nombre.Trim().Length > 150) errs.Add("Nombre: máximo 150 caracteres.");
        }
        if (!string.IsNullOrWhiteSpace(dto.CodigoCie10))
        {
            if (dto.CodigoCie10.Trim().Length > 10) errs.Add("Código CIE-10: máximo 10 caracteres.");
        }
        if (!string.IsNullOrWhiteSpace(dto.Descripcion))
        {
            if (dto.Descripcion.Trim().Length > 300) errs.Add("Descripción: máximo 300 caracteres.");
        }
        return errs;
    }

    private static void Apply(Diagnostico e, AddDiagnosticoDto dto)
    {
        e.Nombre = dto.Nombre.Trim();
        e.CodigoCie10 = string.IsNullOrWhiteSpace(dto.CodigoCie10) ? null : dto.CodigoCie10.Trim();
        e.Descripcion = string.IsNullOrWhiteSpace(dto.Descripcion) ? null : dto.Descripcion.Trim();
    }

    private static DiagnosticoDto ToDto(Diagnostico e) => new()
    {
        Id = e.Id,
        CreationDate = e.CreationDate,
        Nombre = e.Nombre,
        CodigoCie10 = e.CodigoCie10,
        Descripcion = e.Descripcion,
    };
}
