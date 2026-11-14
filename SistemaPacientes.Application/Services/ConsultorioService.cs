using System.Text.RegularExpressions;
using SistemaPacientes.Application.Contract;
using SistemaPacientes.Application.Core;
using SistemaPacientes.Application.Dtos;
using SistemaPacientes.Domain.Entities;
using SistemaPacientes.Infrastructure.Interfaces;

namespace SistemaPacientes.Application.Services;

public class ConsultorioService : BaseService, IConsultorioService
{
    private readonly IConsultorioRepository _repo;

    public ConsultorioService(IConsultorioRepository repo)
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
        return entity is null ? NotFound("Consultorio") : Ok(ToDto(entity));
    });

    public Task<ServiceResult> AddAsync(AddConsultorioDto dto) => ExecuteAsync(async () =>
    {
        if (dto is null) return Fail("No se enviaron datos.");
        var errors = await ValidateAsync(dto, null);
        if (errors.Count > 0) return Fail(errors);

        var entity = new Consultorio();
        Apply(entity, dto);
        await _repo.AddAsync(entity);
        return Ok(ToDto(entity), "Registro creado correctamente.");
    });

    public Task<ServiceResult> UpdateAsync(UpdateConsultorioDto dto) => ExecuteAsync(async () =>
    {
        if (dto is null) return Fail("No se enviaron datos.");
        var entity = await _repo.GetByIdAsync(dto.Id);
        if (entity is null) return NotFound("Consultorio");

        var errors = await ValidateAsync(dto, dto.Id);
        if (errors.Count > 0) return Fail(errors);

        Apply(entity, dto);
        await _repo.UpdateAsync(entity);
        return Ok(ToDto(entity), "Registro actualizado correctamente.");
    });

    public Task<ServiceResult> RemoveAsync(int id) => ExecuteAsync(async () =>
    {
        if (!await _repo.ExistsAsync(id)) return NotFound("Consultorio");
        await _repo.DeleteAsync(id);
        return Ok(null, "Registro eliminado correctamente.");
    });

    // ---- Validaciones de cada campo ----
    private async Task<List<string>> ValidateAsync(AddConsultorioDto dto, int? id)
    {
        var errs = new List<string>();
        if (string.IsNullOrWhiteSpace(dto.Codigo)) errs.Add("Código: campo obligatorio.");
        else
        {
            if (dto.Codigo.Trim().Length > 20) errs.Add("Código: máximo 20 caracteres.");
            if (await _repo.ExistsByCodigoAsync(dto.Codigo.Trim(), id)) errs.Add("Ya existe un registro con ese valor de código.");
        }
        if (!string.IsNullOrWhiteSpace(dto.Ubicacion))
        {
            if (dto.Ubicacion.Trim().Length > 150) errs.Add("Ubicación: máximo 150 caracteres.");
        }
        if (dto.Piso < 0 || dto.Piso > 50) errs.Add("Piso: debe estar entre 0 y 50.");
        return errs;
    }

    private static void Apply(Consultorio e, AddConsultorioDto dto)
    {
        e.Codigo = dto.Codigo.Trim();
        e.Ubicacion = string.IsNullOrWhiteSpace(dto.Ubicacion) ? null : dto.Ubicacion.Trim();
        e.Piso = dto.Piso;
    }

    private static ConsultorioDto ToDto(Consultorio e) => new()
    {
        Id = e.Id,
        CreationDate = e.CreationDate,
        Codigo = e.Codigo,
        Ubicacion = e.Ubicacion,
        Piso = e.Piso,
    };
}
