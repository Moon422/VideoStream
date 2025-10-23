using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using VideoStream.Application.DTOs;
using VideoStream.Application.Interfaces;
using VideoStream.Application.UseCases;

namespace VideoStream.Presentation;

public class WorkContext : IWorkContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly GetUserByEmailUseCase _getUserByEmail;
    private readonly GetUserByUsernameUseCase _getUserByUsername;

    private UserDto? _cachedUserDto = null;

    public WorkContext(IHttpContextAccessor httpContextAccessor,
        GetUserByEmailUseCase getUserByEmail,
        GetUserByUsernameUseCase getUserByUsername)
    {
        _httpContextAccessor = httpContextAccessor;
        _getUserByEmail = getUserByEmail;
        _getUserByUsername = getUserByUsername;
    }

    public async Task<UserDto?> GetCurrentUserAsync()
    {
        if (_cachedUserDto is not null)
            return _cachedUserDto;

        var httpContext = _httpContextAccessor.HttpContext
            ?? throw new InvalidOperationException();

        if (!httpContext.User.Identity?.IsAuthenticated ?? true)
        {
            return null;
        }

        var email = httpContext.User.FindFirstValue(ClaimTypes.Email);

        if (!string.IsNullOrWhiteSpace(email) && (_cachedUserDto = await _getUserByEmail.ExecuteAsync(email)) is not null)
        {
            return _cachedUserDto;
        }

        var username = httpContext.User.FindFirstValue(ClaimTypes.Name);

        if (!string.IsNullOrWhiteSpace(username) && (_cachedUserDto = await _getUserByUsername.ExecuteAsync(username)) is not null)
        {
            return _cachedUserDto;
        }

        return null;
    }
}