using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VideoStream.Application.DTOs;
using VideoStream.Application.UseCases;
using VideoStream.Domain.Interfaces;
using VideoStream.Presentation.Models.Users;

namespace VideoStream.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly CreateUserUseCase _createUserUseCase;

    public UserController(IUserRepository userRepository,
        CreateUserUseCase createUserUseCase)
    {
        _userRepository = userRepository;
        _createUserUseCase = createUserUseCase;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var entity = await _userRepository.GetByIdAsync(id);
        if (entity == null)
            return NotFound();

        var userModel = new UserModel
        {
            Id = entity.Id,
            Firstname = entity.Firstname,
            Lastname = entity.Lastname,
            Username = entity.Username,
            Email = entity.Email,
            IsAdmin = entity.IsAdmin,
            CreatedOn = entity.CreatedOn,
            ModifiedOn = entity.ModifiedOn
        };

        return Ok(userModel);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateUserRequest req)
    {
        var userCreateDto = new CreateUserDto
        {
            Firstname = req.Firstname,
            Lastname = req.Lastname,
            Username = req.Username,
            Email = req.Email,
            Password = req.Password
        };
        var userDto = await _createUserUseCase.ExecuteAsync(userCreateDto);

        var userModel = new UserModel
        {
            Id = userDto.Id,
            Firstname = userDto.Firstname,
            Lastname = userDto.Lastname,
            Username = userDto.Username,
            Email = userDto.Email,
            IsAdmin = userDto.IsAdmin,
            CreatedOn = userDto.CreatedOn,
            ModifiedOn = userDto.ModifiedOn
        };

        return CreatedAtAction(nameof(GetById), new { id = userDto.Id }, userModel);
    }
}
