using System.Text.RegularExpressions;
using SistemaPacientes.Application.Contract;
using SistemaPacientes.Application.Core;
using SistemaPacientes.Application.Dtos;
using SistemaPacientes.Domain.Entities;
using SistemaPacientes.Infrastructure.Interfaces;

namespace SistemaPacientes.Application.Services;

public class EspecialidadService : BaseService, IEspecialidadService
{
    private readonly IEspecialidadRepository _repo;

    public EspecialidadService(IEspecialidadRepository repo)
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
        return entity is null ? NotFound("Especialidad") : Ok(ToDto(entity));
    });

    public Task<ServiceResult> AddAsync(AddEspecialidadDto dto) => ExecuteAsync(async () =>
    {
        if (dto is null) return Fail("No se enviaron datos.");
        var errors = await ValidateAsync(dto, null);
        if (errors.Count > 0) return Fail(errors);

        var entity = new Especialidad();
        Apply(entity, dto);
        await _repo.AddAsync(entity);
        return Ok(ToDto(entity), "Registro creado correctamente.");
    });

    public Task<ServiceResult> UpdateAsync(UpdateEspecialidadDto dto) => ExecuteAsync(async () =>
    {
        if (dto is null) return Fail("No se enviaron datos.");
        var entity = await _repo.GetByIdAsync(dto.Id);
        if (entity is null) return NotFound("Especialidad");

        var errors = await ValidateAsync(dto, dto.Id);
        if (errors.Count > 0) return Fail(errors);

        Apply(entity, dto);
        await _repo.UpdateAsync(entity);
        return Ok(ToDto(entity), "Registro actualizado correctamente.");
    });

    public Task<ServiceResult> RemoveAsync(int id) => ExecuteAsync(async () =>
    {
        if (!await _repo.ExistsAsync(id)) return NotFound("Especialidad");
        await _repo.DeleteAsync(id);
        return Ok(null, "Registro eliminado correctamente.");
    });

    // ---- Validaciones de cada campo ----
    private async Task<List<string>> ValidateAsync(AddEspecialidadDto dto, int? id)
    {
        var errs = new List<string>();
        if (string.IsNullOrWhiteSpace(dto.Nombre)) errs.Add("Nombre: campo obligatorio.");
        else
        {
            if (dto.Nombre.Trim().Length > 100) errs.Add("Nombre: máximo 100 caracteres.");
            if (await _repo.ExistsByNombreAsync(dto.Nombre.Trim(), id)) errs.Add("Ya existe un registro con ese valor de nombre.");
        }
        if (!string.IsNullOrWhiteSpace(dto.Descripcion))
        {
            if (dto.Descripcion.Trim().Length > 300) errs.Add("Descripción: máximo 300 caracteres.");
        }
        return errs;
    }

    private static void Apply(Especialidad e, AddEspecialidadDto dto)
    {
        e.Nombre = dto.Nombre.Trim();
        e.Descripcion = string.IsNullOrWhiteSpace(dto.Descripcion) ? null : dto.Descripcion.Trim();
    }

    private static EspecialidadDto ToDto(Especialidad e) => new()
    {
        Id = e.Id,
        CreationDate = e.CreationDate,
        Nombre = e.Nombre,
        Descripcion = e.Descripcion,
    };
}
