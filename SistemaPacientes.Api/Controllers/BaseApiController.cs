using Microsoft.AspNetCore.Mvc;
using SistemaPacientes.Application.Core;

namespace SistemaPacientes.Api.Controllers;

[ApiController]
public abstract class BaseApiController : ControllerBase
{
    protected IActionResult FromResult(ServiceResult result)
    {
        if (result.Success) return Ok(result);
        if (result.NotFound) return NotFound(result);
        return BadRequest(result);
    }
}
