using SistemaPacientes.Application.Core;
using SistemaPacientes.Application.Dtos;

namespace SistemaPacientes.Application.Contract;

public interface IConsultaService : IBaseService<AddConsultaDto, UpdateConsultaDto>
{
}
