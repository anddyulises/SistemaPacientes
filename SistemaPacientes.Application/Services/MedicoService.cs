using System.Text.RegularExpressions;
using SistemaPacientes.Application.Contract;
using SistemaPacientes.Application.Core;
using SistemaPacientes.Application.Dtos;
using SistemaPacientes.Domain.Entities;
using SistemaPacientes.Infrastructure.Interfaces;

namespace SistemaPacientes.Application.Services;

public class MedicoService : BaseService, IMedicoService
{
    private readonly IMedicoRepository _repo;
    private readonly IEspecialidadRepository _especialidadRepo;

    public MedicoService(IMedicoRepository repo, IEspecialidadRepository especialidadRepo)
    {
        _repo = repo;
        _especialidadRepo = especialidadRepo;
    }

    public Task<ServiceResult> GetAllAsync() => ExecuteAsync(async () =>
    {
        var list = await _repo.GetAllAsync();
        return Ok(list.Select(ToDto).ToList());
    });

    public Task<ServiceResult> GetByIdAsync(int id) => ExecuteAsync(async () =>
    {
        var entity = await _repo.GetByIdAsync(id);
        return entity is null ? NotFound("Medico") : Ok(ToDto(entity));
    });

    public Task<ServiceResult> AddAsync(AddMedicoDto dto) => ExecuteAsync(async () =>
    {
        if (dto is null) return Fail("No se enviaron datos.");
        var errors = await ValidateAsync(dto, null);
        if (errors.Count > 0) return Fail(errors);

        var entity = new Medico();
        Apply(entity, dto);
        await _repo.AddAsync(entity);
        return Ok(ToDto(entity), "Registro creado correctamente.");
    });

    public Task<ServiceResult> UpdateAsync(UpdateMedicoDto dto) => ExecuteAsync(async () =>
    {
        if (dto is null) return Fail("No se enviaron datos.");
        var entity = await _repo.GetByIdAsync(dto.Id);
        if (entity is null) return NotFound("Medico");

        var errors = await ValidateAsync(dto, dto.Id);
        if (errors.Count > 0) return Fail(errors);

        Apply(entity, dto);
        await _repo.UpdateAsync(entity);
        return Ok(ToDto(entity), "Registro actualizado correctamente.");
    });

    public Task<ServiceResult> RemoveAsync(int id) => ExecuteAsync(async () =>
    {
        if (!await _repo.ExistsAsync(id)) return NotFound("Medico");
        await _repo.DeleteAsync(id);
        return Ok(null, "Registro eliminado correctamente.");
    });

    // ---- Validaciones de cada campo ----
    private async Task<List<string>> ValidateAsync(AddMedicoDto dto, int? id)
    {
        var errs = new List<string>();
        if (string.IsNullOrWhiteSpace(dto.Nombres)) errs.Add("Nombres: campo obligatorio.");
        else
        {
            if (dto.Nombres.Trim().Length > 100) errs.Add("Nombres: máximo 100 caracteres.");
        }
        if (string.IsNullOrWhiteSpace(dto.Apellidos)) errs.Add("Apellidos: campo obligatorio.");
        else
        {
            if (dto.Apellidos.Trim().Length > 100) errs.Add("Apellidos: máximo 100 caracteres.");
        }
        if (string.IsNullOrWhiteSpace(dto.Exequatur)) errs.Add("Exequátur: campo obligatorio.");
        else
        {
            if (dto.Exequatur.Trim().Length > 30) errs.Add("Exequátur: máximo 30 caracteres.");
            if (await _repo.ExistsByExequaturAsync(dto.Exequatur.Trim(), id)) errs.Add("Ya existe un registro con ese valor de exequátur.");
        }
        if (!string.IsNullOrWhiteSpace(dto.Telefono))
        {
            if (dto.Telefono.Trim().Length > 20) errs.Add("Teléfono: máximo 20 caracteres.");
            if (!Regex.IsMatch(dto.Telefono.Trim(), @"^[0-9+\-() ]{7,20}$")) errs.Add("Teléfono: teléfono no válido (7 a 20 dígitos).");
        }
        if (!string.IsNullOrWhiteSpace(dto.Email))
        {
            if (dto.Email.Trim().Length > 150) errs.Add("Correo: máximo 150 caracteres.");
            if (!Regex.IsMatch(dto.Email.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$")) errs.Add("Correo: formato de correo no válido.");
        }
        if (dto.EspecialidadId <= 0) errs.Add("Especialidad: campo obligatorio.");
        else if (!await _especialidadRepo.ExistsAsync(dto.EspecialidadId)) errs.Add("Especialidad: el registro seleccionado no existe.");
        return errs;
    }

    private static void Apply(Medico e, AddMedicoDto dto)
    {
        e.Nombres = dto.Nombres.Trim();
        e.Apellidos = dto.Apellidos.Trim();
        e.Exequatur = dto.Exequatur.Trim();
        e.Telefono = string.IsNullOrWhiteSpace(dto.Telefono) ? null : dto.Telefono.Trim();
        e.Email = string.IsNullOrWhiteSpace(dto.Email) ? null : dto.Email.Trim();
        e.EspecialidadId = dto.EspecialidadId;
    }

    private static MedicoDto ToDto(Medico e) => new()
    {
        Id = e.Id,
        CreationDate = e.CreationDate,
        Nombres = e.Nombres,
        Apellidos = e.Apellidos,
        Exequatur = e.Exequatur,
        Telefono = e.Telefono,
        Email = e.Email,
        EspecialidadId = e.EspecialidadId,
    };
}
