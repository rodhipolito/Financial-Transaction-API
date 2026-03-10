using System.Security.Claims;
using FinancialTransactionAPI.Models;
using FinancialTransactionAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinancialTransactionAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AccountsController : ControllerBase
{
    private readonly AccountService _accountService;

    public AccountsController(AccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAccountRequest request)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var account = await _accountService.CreateAsync(userId, request.Type, request.Currency);
        return Ok(account);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var accounts = await _accountService.GetByUserAsync(userId);
        return Ok(accounts);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var account = await _accountService.GetByIdAsync(id, userId);

        if (account == null)
            return NotFound(new { message = "Account not found." });

        return Ok(account);
    }
}

public record CreateAccountRequest(AccountType Type, string Currency);