using System.Text.RegularExpressions;
using SistemaPacientes.Application.Contract;
using SistemaPacientes.Application.Core;
using SistemaPacientes.Application.Dtos;
using SistemaPacientes.Domain.Entities;
using SistemaPacientes.Infrastructure.Interfaces;

namespace SistemaPacientes.Application.Services;

public class SeguroService : BaseService, ISeguroService
{
    private readonly ISeguroRepository _repo;

    public SeguroService(ISeguroRepository repo)
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
        return entity is null ? NotFound("Seguro") : Ok(ToDto(entity));
    });

    public Task<ServiceResult> AddAsync(AddSeguroDto dto) => ExecuteAsync(async () =>
    {
        if (dto is null) return Fail("No se enviaron datos.");
        var errors = await ValidateAsync(dto, null);
        if (errors.Count > 0) return Fail(errors);

        var entity = new Seguro();
        Apply(entity, dto);
        await _repo.AddAsync(entity);
        return Ok(ToDto(entity), "Registro creado correctamente.");
    });

    public Task<ServiceResult> UpdateAsync(UpdateSeguroDto dto) => ExecuteAsync(async () =>
    {
        if (dto is null) return Fail("No se enviaron datos.");
        var entity = await _repo.GetByIdAsync(dto.Id);
        if (entity is null) return NotFound("Seguro");

        var errors = await ValidateAsync(dto, dto.Id);
        if (errors.Count > 0) return Fail(errors);

        Apply(entity, dto);
        await _repo.UpdateAsync(entity);
        return Ok(ToDto(entity), "Registro actualizado correctamente.");
    });

    public Task<ServiceResult> RemoveAsync(int id) => ExecuteAsync(async () =>
    {
        if (!await _repo.ExistsAsync(id)) return NotFound("Seguro");
        await _repo.DeleteAsync(id);
        return Ok(null, "Registro eliminado correctamente.");
    });

    // ---- Validaciones de cada campo ----
    private async Task<List<string>> ValidateAsync(AddSeguroDto dto, int? id)
    {
        var errs = new List<string>();
        if (string.IsNullOrWhiteSpace(dto.Nombre)) errs.Add("Nombre: campo obligatorio.");
        else
        {
            if (dto.Nombre.Trim().Length > 100) errs.Add("Nombre: máximo 100 caracteres.");
            if (await _repo.ExistsByNombreAsync(dto.Nombre.Trim(), id)) errs.Add("Ya existe un registro con ese valor de nombre.");
        }
        if (!string.IsNullOrWhiteSpace(dto.Telefono))
        {
            if (dto.Telefono.Trim().Length > 20) errs.Add("Teléfono: máximo 20 caracteres.");
            if (!Regex.IsMatch(dto.Telefono.Trim(), @"^[0-9+\-() ]{7,20}$")) errs.Add("Teléfono: teléfono no válido (7 a 20 dígitos).");
        }
        if (dto.Cobertura < 0m || dto.Cobertura > 100m) errs.Add("Cobertura (%): debe estar entre 0 y 100.");
        return errs;
    }

    private static void Apply(Seguro e, AddSeguroDto dto)
    {
        e.Nombre = dto.Nombre.Trim();
        e.Telefono = string.IsNullOrWhiteSpace(dto.Telefono) ? null : dto.Telefono.Trim();
        e.Cobertura = dto.Cobertura;
    }

    private static SeguroDto ToDto(Seguro e) => new()
    {
        Id = e.Id,
        CreationDate = e.CreationDate,
        Nombre = e.Nombre,
        Telefono = e.Telefono,
        Cobertura = e.Cobertura,
    };
}
