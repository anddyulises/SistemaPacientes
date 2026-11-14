using System.Text.RegularExpressions;
using SistemaPacientes.Application.Contract;
using SistemaPacientes.Application.Core;
using SistemaPacientes.Application.Dtos;
using SistemaPacientes.Domain.Entities;
using SistemaPacientes.Infrastructure.Interfaces;

namespace SistemaPacientes.Application.Services;

public class RecetaDetalleService : BaseService, IRecetaDetalleService
{
    private readonly IRecetaDetalleRepository _repo;
    private readonly IRecetaRepository _recetaRepo;
    private readonly IMedicamentoRepository _medicamentoRepo;

    public RecetaDetalleService(IRecetaDetalleRepository repo, IRecetaRepository recetaRepo, IMedicamentoRepository medicamentoRepo)
    {
        _repo = repo;
        _recetaRepo = recetaRepo;
        _medicamentoRepo = medicamentoRepo;
    }

    public Task<ServiceResult> GetAllAsync() => ExecuteAsync(async () =>
    {
        var list = await _repo.GetAllAsync();
        return Ok(list.Select(ToDto).ToList());
    });

    public Task<ServiceResult> GetByIdAsync(int id) => ExecuteAsync(async () =>
    {
        var entity = await _repo.GetByIdAsync(id);
        return entity is null ? NotFound("RecetaDetalle") : Ok(ToDto(entity));
    });

    public Task<ServiceResult> AddAsync(AddRecetaDetalleDto dto) => ExecuteAsync(async () =>
    {
        if (dto is null) return Fail("No se enviaron datos.");
        var errors = await ValidateAsync(dto, null);
        if (errors.Count > 0) return Fail(errors);

        var entity = new RecetaDetalle();
        Apply(entity, dto);
        await _repo.AddAsync(entity);
        return Ok(ToDto(entity), "Registro creado correctamente.");
    });

    public Task<ServiceResult> UpdateAsync(UpdateRecetaDetalleDto dto) => ExecuteAsync(async () =>
    {
        if (dto is null) return Fail("No se enviaron datos.");
        var entity = await _repo.GetByIdAsync(dto.Id);
        if (entity is null) return NotFound("RecetaDetalle");

        var errors = await ValidateAsync(dto, dto.Id);
        if (errors.Count > 0) return Fail(errors);

        Apply(entity, dto);
        await _repo.UpdateAsync(entity);
        return Ok(ToDto(entity), "Registro actualizado correctamente.");
    });

    public Task<ServiceResult> RemoveAsync(int id) => ExecuteAsync(async () =>
    {
        if (!await _repo.ExistsAsync(id)) return NotFound("RecetaDetalle");
        await _repo.DeleteAsync(id);
        return Ok(null, "Registro eliminado correctamente.");
    });

    // ---- Validaciones de cada campo ----
    private async Task<List<string>> ValidateAsync(AddRecetaDetalleDto dto, int? id)
    {
        var errs = new List<string>();
        if (dto.RecetaId <= 0) errs.Add("Receta: campo obligatorio.");
        else if (!await _recetaRepo.ExistsAsync(dto.RecetaId)) errs.Add("Receta: el registro seleccionado no existe.");
        if (dto.MedicamentoId <= 0) errs.Add("Medicamento: campo obligatorio.");
        else if (!await _medicamentoRepo.ExistsAsync(dto.MedicamentoId)) errs.Add("Medicamento: el registro seleccionado no existe.");
        if (string.IsNullOrWhiteSpace(dto.Dosis)) errs.Add("Dosis: campo obligatorio.");
        else
        {
            if (dto.Dosis.Trim().Length > 100) errs.Add("Dosis: máximo 100 caracteres.");
        }
        if (dto.DuracionDias < 1 || dto.DuracionDias > 365) errs.Add("Duración (días): debe estar entre 1 y 365.");
        return errs;
    }

    private static void Apply(RecetaDetalle e, AddRecetaDetalleDto dto)
    {
        e.RecetaId = dto.RecetaId;
        e.MedicamentoId = dto.MedicamentoId;
        e.Dosis = dto.Dosis.Trim();
        e.DuracionDias = dto.DuracionDias;
    }

    private static RecetaDetalleDto ToDto(RecetaDetalle e) => new()
    {
        Id = e.Id,
        CreationDate = e.CreationDate,
        RecetaId = e.RecetaId,
        MedicamentoId = e.MedicamentoId,
        Dosis = e.Dosis,
        DuracionDias = e.DuracionDias,
    };
}
