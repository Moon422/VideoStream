using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using VideoStream.Application.DTOs;
using VideoStream.Application.UseCases;
using VideoStream.Presentation.Models.Auth;

namespace VideoStream.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly VerifyUserLoginUseCase _verifyUserLoginUseCase;
    private readonly IConfiguration _configuration;

    public AuthController(VerifyUserLoginUseCase verifyUserLoginUseCase,
        IConfiguration configuration)
    {
        _verifyUserLoginUseCase = verifyUserLoginUseCase;
        _configuration = configuration;
    }

    private async Task<string> GenerateJwtToken(UserDto user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email)
        };

        var secret = _configuration.GetSection("Secret").Value
            ?? throw new InvalidOperationException();

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
        var token = new JwtSecurityToken(claims: claims, expires: DateTime.UtcNow.AddMinutes(5), signingCredentials: creds);
        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        if (HttpContext is not null)
        {
            HttpContext.Response.Cookies.Append("jwt", jwt, new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTimeOffset.UtcNow.AddMinutes(5)
            });
        }

        return jwt;
    }

    private async Task<string> LoginAsync(UserDto user)
    {
        var jwt = await _authService.GenerateJwtToken(user);
        await _authService.GenerateRefreshToken(user);

        user.LastLogin = DateTime.UtcNow;
        await _userService.UpdateUserAsync(user);

        return jwt;
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        try
        {
            var userDto = await _verifyUserLoginUseCase.ExecuteAsync(model.Email, model.Password);
        }
        catch (InvalidCredentialException ex)
        {
            return BadRequest(ex.Message);
        }

        var userModel = user.ToModel();
        var jwt = await LoginAsync(user);
        var loginResponse = new LoginResponseModel
        {
            User = userModel,
            Jwt = jwt
        };

        response.Data = loginResponse;

        return Ok(response);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Register([FromBody] RegistrationModel model)
    {
        var user = (await _userService.GetUserByEmailAsync(model.Email))
            ?? await _userService.GetUserByUseranmeAsync(model.Username);

        if (user is not null)
        {
            return BadRequest("User with email or username already exists.");
        }

        user = model.ToEntity();
        try
        {
            await _transactionManager.ExecuteAsync(async () =>
            {
                await _userService.CreateUserAsync(user);
                await _userService.CreatePasswordAsync(user.Id, model.Password);
            });

            var userModel = user.ToModel();
            var jwt = await LoginAsync(user);
            var response = new LoginResponseModel
            {
                User = userModel,
                Jwt = jwt
            };

            return CreatedAtAction(nameof(Details), response);
        }
        catch
        {
            return Problem();
        }
    }

    [HttpGet("refresh-token")]
    public async Task<IActionResult> RefreshToken()
    {
        var token = HttpContext.Request.Cookies["refresh-token"];
        var response = new HttpResponseModel<LoginResponseModel>();
        if (string.IsNullOrWhiteSpace(token))
        {
            response.StatusCode = StatusCodes.Status401Unauthorized;
            response.Errors.Add("Invalid authentication token.");

            return Unauthorized(response);
        }

        var refreshToken = await _refreshTokenService.GetRefreshTokenByTokenAsync(token);
        if (refreshToken is null)
        {
            response.StatusCode = StatusCodes.Status401Unauthorized;
            response.Errors.Add("Invalid authentication token.");

            return Unauthorized(response);
        }

        if (!refreshToken.IsValid)
        {
            response.StatusCode = StatusCodes.Status401Unauthorized;
            response.Errors.Add("Invalid authentication token.");

            return Unauthorized(response);
        }

        var user = await _userService.GetUserByIdAsync(refreshToken.UserId);
        if (user is null)
        {
            response.StatusCode = StatusCodes.Status400BadRequest;
            response.Errors.Add("User not found.");

            return BadRequest(response);
        }

        var expirationDurationRemaining = refreshToken.ExpiryDate - DateTime.UtcNow;
        if (expirationDurationRemaining < TimeSpan.Zero)
        {
            response.StatusCode = StatusCodes.Status401Unauthorized;
            response.Errors.Add("Invalid authentication token.");

            return Unauthorized(response);
        }

        var expirationHourRemaining = expirationDurationRemaining.TotalHours;
        if (expirationHourRemaining <= 24)
        {
            await _authService.GenerateRefreshToken(user);
        }

        var userModel = user.ToModel();
        var jwt = await _authService.GenerateJwtToken(user);
        var loginResponse = new LoginResponseModel
        {
            User = userModel,
            Jwt = jwt
        };

        response.StatusCode = StatusCodes.Status200OK;
        response.Data = loginResponse;

        return Ok(response);
    }
}