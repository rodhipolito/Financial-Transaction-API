using FinancialTransactionAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinancialTransactionAPI.Controllers;

/// <summary>
/// Handles user authentication including registration and login.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Registers a new user account.
    /// </summary>
    /// <remarks>
    /// After registering, use the returned JWT token to authenticate requests.
    /// Include the token in the Authorize button above as: Bearer {token}
    /// </remarks>
    /// <param name="request">User registration details</param>
    /// <returns>JWT token for authentication</returns>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var token = await _authService.RegisterAsync(request.Name, request.Email, request.Password);

        if (token == null)
            return BadRequest(new { message = "Email already in use." });

        return Ok(new { token });
    }

    /// <summary>
    /// Authenticates a user and returns a JWT token.
    /// </summary>
    /// <remarks>
    /// Copy the returned token and click the Authorize button above.
    /// Enter: Bearer {your_token_here}
    /// </remarks>
    /// <param name="request">User login credentials</param>
    /// <returns>JWT token for authentication</returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var token = await _authService.LoginAsync(request.Email, request.Password);

        if (token == null)
            return Unauthorized(new { message = "Invalid credentials." });

        return Ok(new { token });
    }
}

public record RegisterRequest(string Name, string Email, string Password);
public record LoginRequest(string Email, string Password);