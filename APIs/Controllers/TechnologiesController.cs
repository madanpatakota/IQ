using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Misard.IQs.Application.Interfaces.Repositories;
using Misard.IQs.Domain.Entities;

namespace Misard.IQs.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TechnologiesController : ControllerBase
{
    private readonly ITechnologyRepository _techRepo;

    public TechnologiesController(ITechnologyRepository techRepo)
    {
        _techRepo = techRepo;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<Technology>>> GetAll()
    {
        var data = await _techRepo.GetAllAsync();
        return Ok(data);
    }
}
