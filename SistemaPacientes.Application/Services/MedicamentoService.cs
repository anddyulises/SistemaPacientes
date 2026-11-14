using System.Text.RegularExpressions;
using SistemaPacientes.Application.Contract;
using SistemaPacientes.Application.Core;
using SistemaPacientes.Application.Dtos;
using SistemaPacientes.Domain.Entities;
using SistemaPacientes.Infrastructure.Interfaces;

namespace SistemaPacientes.Application.Services;

public class MedicamentoService : BaseService, IMedicamentoService
{
    private readonly IMedicamentoRepository _repo;

    public MedicamentoService(IMedicamentoRepository repo)
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
        return entity is null ? NotFound("Medicamento") : Ok(ToDto(entity));
    });

    public Task<ServiceResult> AddAsync(AddMedicamentoDto dto) => ExecuteAsync(async () =>
    {
        if (dto is null) return Fail("No se enviaron datos.");
        var errors = await ValidateAsync(dto, null);
        if (errors.Count > 0) return Fail(errors);

        var entity = new Medicamento();
        Apply(entity, dto);
        await _repo.AddAsync(entity);
        return Ok(ToDto(entity), "Registro creado correctamente.");
    });

    public Task<ServiceResult> UpdateAsync(UpdateMedicamentoDto dto) => ExecuteAsync(async () =>
    {
        if (dto is null) return Fail("No se enviaron datos.");
        var entity = await _repo.GetByIdAsync(dto.Id);
        if (entity is null) return NotFound("Medicamento");

        var errors = await ValidateAsync(dto, dto.Id);
        if (errors.Count > 0) return Fail(errors);

        Apply(entity, dto);
        await _repo.UpdateAsync(entity);
        return Ok(ToDto(entity), "Registro actualizado correctamente.");
    });

    public Task<ServiceResult> RemoveAsync(int id) => ExecuteAsync(async () =>
    {
        if (!await _repo.ExistsAsync(id)) return NotFound("Medicamento");
        await _repo.DeleteAsync(id);
        return Ok(null, "Registro eliminado correctamente.");
    });

    // ---- Validaciones de cada campo ----
    private async Task<List<string>> ValidateAsync(AddMedicamentoDto dto, int? id)
    {
        var errs = new List<string>();
        if (string.IsNullOrWhiteSpace(dto.Nombre)) errs.Add("Nombre: campo obligatorio.");
        else
        {
            if (dto.Nombre.Trim().Length > 150) errs.Add("Nombre: máximo 150 caracteres.");
        }
        if (!string.IsNullOrWhiteSpace(dto.Presentacion))
        {
            if (dto.Presentacion.Trim().Length > 100) errs.Add("Presentación: máximo 100 caracteres.");
        }
        if (dto.Stock < 0 || dto.Stock > 1000000) errs.Add("Stock: debe estar entre 0 y 1000000.");
        return errs;
    }

    private static void Apply(Medicamento e, AddMedicamentoDto dto)
    {
        e.Nombre = dto.Nombre.Trim();
        e.Presentacion = string.IsNullOrWhiteSpace(dto.Presentacion) ? null : dto.Presentacion.Trim();
        e.Stock = dto.Stock;
    }

    private static MedicamentoDto ToDto(Medicamento e) => new()
    {
        Id = e.Id,
        CreationDate = e.CreationDate,
        Nombre = e.Nombre,
        Presentacion = e.Presentacion,
        Stock = e.Stock,
    };
}
