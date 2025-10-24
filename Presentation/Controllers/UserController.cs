using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VideoStream.Application.UseCases;
using VideoStream.Presentation.Models.Users;

namespace VideoStream.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly CreateUserUseCase _createUserUseCase;
    private readonly GetUserByIdUseCase _getUserByIdUseCase;

    public UserController(CreateUserUseCase createUserUseCase,
        GetUserByIdUseCase getUserByIdUseCase)
    {
        _createUserUseCase = createUserUseCase;
        _getUserByIdUseCase = getUserByIdUseCase;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var userDto = await _getUserByIdUseCase.ExecuteAsync(id);
        if (userDto == null)
            return NotFound();

        return Ok(userDto.ToUserModel());
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateUserRequest req)
    {
        try
        {
            var userDto = await _createUserUseCase.ExecuteAsync(req.ToCreateUserDto());
            return CreatedAtAction(nameof(GetById), new { id = userDto.Id }, userDto.ToUserModel());
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> AccountDetails()
    {
        return Ok();
    }
}
