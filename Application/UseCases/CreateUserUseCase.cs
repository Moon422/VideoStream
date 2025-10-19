using System.Threading.Tasks;
using VideoStream.Application.DTOs;
using VideoStream.Domain.Entities;

namespace VideoStream.Application.UseCases;

public class CreateUserUseCase
{
    public async Task ExecuteAsync(UserCreateDto request)
    {
        var user = new User
        {
            Firstname = request.Firstname,
            Lastname = request.Lastname,
            Username = request.Username,
            Email = request.Email,

        }
    }
}