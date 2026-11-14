using System.Text.RegularExpressions;
using SistemaPacientes.Application.Contract;
using SistemaPacientes.Application.Core;
using SistemaPacientes.Application.Dtos;
using SistemaPacientes.Domain.Entities;
using SistemaPacientes.Infrastructure.Interfaces;

namespace SistemaPacientes.Application.Services;

public class RecetaService : BaseService, IRecetaService
{
    private readonly IRecetaRepository _repo;
    private readonly IConsultaRepository _consultaRepo;

    public RecetaService(IRecetaRepository repo, IConsultaRepository consultaRepo)
    {
        _repo = repo;
        _consultaRepo = consultaRepo;
    }

    public Task<ServiceResult> GetAllAsync() => ExecuteAsync(async () =>
    {
        var list = await _repo.GetAllAsync();
        return Ok(list.Select(ToDto).ToList());
    });

    public Task<ServiceResult> GetByIdAsync(int id) => ExecuteAsync(async () =>
    {
        var entity = await _repo.GetByIdAsync(id);
        return entity is null ? NotFound("Receta") : Ok(ToDto(entity));
    });

    public Task<ServiceResult> AddAsync(AddRecetaDto dto) => ExecuteAsync(async () =>
    {
        if (dto is null) return Fail("No se enviaron datos.");
        var errors = await ValidateAsync(dto, null);
        if (errors.Count > 0) return Fail(errors);

        var entity = new Receta();
        Apply(entity, dto);
        await _repo.AddAsync(entity);
        return Ok(ToDto(entity), "Registro creado correctamente.");
    });

    public Task<ServiceResult> UpdateAsync(UpdateRecetaDto dto) => ExecuteAsync(async () =>
    {
        if (dto is null) return Fail("No se enviaron datos.");
        var entity = await _repo.GetByIdAsync(dto.Id);
        if (entity is null) return NotFound("Receta");

        var errors = await ValidateAsync(dto, dto.Id);
        if (errors.Count > 0) return Fail(errors);

        Apply(entity, dto);
        await _repo.UpdateAsync(entity);
        return Ok(ToDto(entity), "Registro actualizado correctamente.");
    });

    public Task<ServiceResult> RemoveAsync(int id) => ExecuteAsync(async () =>
    {
        if (!await _repo.ExistsAsync(id)) return NotFound("Receta");
        await _repo.DeleteAsync(id);
        return Ok(null, "Registro eliminado correctamente.");
    });

    // ---- Validaciones de cada campo ----
    private async Task<List<string>> ValidateAsync(AddRecetaDto dto, int? id)
    {
        var errs = new List<string>();
        if (dto.ConsultaId <= 0) errs.Add("Consulta: campo obligatorio.");
        else if (!await _consultaRepo.ExistsAsync(dto.ConsultaId)) errs.Add("Consulta: el registro seleccionado no existe.");
        if (dto.Fecha.Year < 1900) errs.Add("Fecha: campo obligatorio.");
        if (!string.IsNullOrWhiteSpace(dto.Indicaciones))
        {
            if (dto.Indicaciones.Trim().Length > 300) errs.Add("Indicaciones: máximo 300 caracteres.");
        }
        return errs;
    }

    private static void Apply(Receta e, AddRecetaDto dto)
    {
        e.ConsultaId = dto.ConsultaId;
        e.Fecha = dto.Fecha;
        e.Indicaciones = string.IsNullOrWhiteSpace(dto.Indicaciones) ? null : dto.Indicaciones.Trim();
    }

    private static RecetaDto ToDto(Receta e) => new()
    {
        Id = e.Id,
        CreationDate = e.CreationDate,
        ConsultaId = e.ConsultaId,
        Fecha = e.Fecha,
        Indicaciones = e.Indicaciones,
    };
}
