using Microsoft.AspNetCore.Mvc;
using SistemaPacientes.Application.Contract;
using SistemaPacientes.Application.Dtos;

namespace SistemaPacientes.Api.Controllers;

[Route("api/recetas")]
public class RecetasController : BaseApiController
{
    private readonly IRecetaService _service;

    public RecetasController(IRecetaService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll() => FromResult(await _service.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id) => FromResult(await _service.GetByIdAsync(id));

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] AddRecetaDto dto) => FromResult(await _service.AddAsync(dto));

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateRecetaDto dto)
    {
        if (dto is not null) dto.Id = id;
        return FromResult(await _service.UpdateAsync(dto!));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Remove(int id) => FromResult(await _service.RemoveAsync(id));
}
