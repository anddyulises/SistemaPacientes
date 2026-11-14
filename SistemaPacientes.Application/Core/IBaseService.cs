namespace SistemaPacientes.Application.Core;

public interface IBaseService<TAddDto, TUpdateDto>
{
    Task<ServiceResult> GetAllAsync();
    Task<ServiceResult> GetByIdAsync(int id);
    Task<ServiceResult> AddAsync(TAddDto dto);
    Task<ServiceResult> UpdateAsync(TUpdateDto dto);
    Task<ServiceResult> RemoveAsync(int id);
}
