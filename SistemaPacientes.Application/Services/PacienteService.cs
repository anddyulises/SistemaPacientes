using System.Text.RegularExpressions;
using SistemaPacientes.Application.Contract;
using SistemaPacientes.Application.Core;
using SistemaPacientes.Application.Dtos;
using SistemaPacientes.Domain.Entities;
using SistemaPacientes.Infrastructure.Interfaces;

namespace SistemaPacientes.Application.Services;

public class PacienteService : BaseService, IPacienteService
{
    private readonly IPacienteRepository _repo;
    private readonly ISeguroRepository _seguroRepo;

    public PacienteService(IPacienteRepository repo, ISeguroRepository seguroRepo)
    {
        _repo = repo;
        _seguroRepo = seguroRepo;
    }

    public Task<ServiceResult> GetAllAsync() => ExecuteAsync(async () =>
    {
        var list = await _repo.GetAllAsync();
        return Ok(list.Select(ToDto).ToList());
    });

    public Task<ServiceResult> GetByIdAsync(int id) => ExecuteAsync(async () =>
    {
        var entity = await _repo.GetByIdAsync(id);
        return entity is null ? NotFound("Paciente") : Ok(ToDto(entity));
    });

    public Task<ServiceResult> AddAsync(AddPacienteDto dto) => ExecuteAsync(async () =>
    {
        if (dto is null) return Fail("No se enviaron datos.");
        var errors = await ValidateAsync(dto, null);
        if (errors.Count > 0) return Fail(errors);

        var entity = new Paciente();
        Apply(entity, dto);
        await _repo.AddAsync(entity);
        return Ok(ToDto(entity), "Registro creado correctamente.");
    });

    public Task<ServiceResult> UpdateAsync(UpdatePacienteDto dto) => ExecuteAsync(async () =>
    {
        if (dto is null) return Fail("No se enviaron datos.");
        var entity = await _repo.GetByIdAsync(dto.Id);
        if (entity is null) return NotFound("Paciente");

        var errors = await ValidateAsync(dto, dto.Id);
        if (errors.Count > 0) return Fail(errors);

        Apply(entity, dto);
        await _repo.UpdateAsync(entity);
        return Ok(ToDto(entity), "Registro actualizado correctamente.");
    });

    public Task<ServiceResult> RemoveAsync(int id) => ExecuteAsync(async () =>
    {
        if (!await _repo.ExistsAsync(id)) return NotFound("Paciente");
        await _repo.DeleteAsync(id);
        return Ok(null, "Registro eliminado correctamente.");
    });

    // ---- Validaciones de cada campo ----
    private async Task<List<string>> ValidateAsync(AddPacienteDto dto, int? id)
    {
        var errs = new List<string>();
        if (string.IsNullOrWhiteSpace(dto.Cedula)) errs.Add("Cédula: campo obligatorio.");
        else
        {
            if (dto.Cedula.Trim().Length > 11) errs.Add("Cédula: máximo 11 caracteres.");
            if (!Regex.IsMatch(dto.Cedula.Trim(), @"^\d{11}$")) errs.Add("Cédula: debe tener 11 dígitos, sin guiones.");
            if (await _repo.ExistsByCedulaAsync(dto.Cedula.Trim(), id)) errs.Add("Ya existe un registro con ese valor de cédula.");
        }
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
        if (dto.FechaNacimiento.Year < 1900) errs.Add("Fecha de nacimiento: campo obligatorio.");
        else if (dto.FechaNacimiento.Date > DateTime.Today) errs.Add("Fecha de nacimiento: no puede ser una fecha futura.");
        if (string.IsNullOrWhiteSpace(dto.Sexo) || !new[] { "M", "F" }.Contains(dto.Sexo)) errs.Add("Sexo: valores permitidos: M, F.");
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
        if (!string.IsNullOrWhiteSpace(dto.Direccion))
        {
            if (dto.Direccion.Trim().Length > 200) errs.Add("Dirección: máximo 200 caracteres.");
        }
        if (dto.SeguroId.HasValue && !await _seguroRepo.ExistsAsync(dto.SeguroId.Value)) errs.Add("Seguro: el registro seleccionado no existe.");
        return errs;
    }

    private static void Apply(Paciente e, AddPacienteDto dto)
    {
        e.Cedula = dto.Cedula.Trim();
        e.Nombres = dto.Nombres.Trim();
        e.Apellidos = dto.Apellidos.Trim();
        e.FechaNacimiento = dto.FechaNacimiento;
        e.Sexo = dto.Sexo;
        e.Telefono = string.IsNullOrWhiteSpace(dto.Telefono) ? null : dto.Telefono.Trim();
        e.Email = string.IsNullOrWhiteSpace(dto.Email) ? null : dto.Email.Trim();
        e.Direccion = string.IsNullOrWhiteSpace(dto.Direccion) ? null : dto.Direccion.Trim();
        e.SeguroId = dto.SeguroId;
    }

    private static PacienteDto ToDto(Paciente e) => new()
    {
        Id = e.Id,
        CreationDate = e.CreationDate,
        Cedula = e.Cedula,
        Nombres = e.Nombres,
        Apellidos = e.Apellidos,
        FechaNacimiento = e.FechaNacimiento,
        Sexo = e.Sexo,
        Telefono = e.Telefono,
        Email = e.Email,
        Direccion = e.Direccion,
        SeguroId = e.SeguroId,
    };
}
