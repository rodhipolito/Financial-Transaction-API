using System.Security.Claims;
using FinancialTransactionAPI.Models;
using FinancialTransactionAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinancialTransactionAPI.Controllers;

/// <summary>
/// Manages financial accounts for authenticated users.
/// </summary>
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

    /// <summary>
    /// Creates a new financial account for the authenticated user.
    /// </summary>
    /// <remarks>
    /// Account types: 0 = Checking, 1 = Savings, 2 = Investment
    /// </remarks>
    /// <param name="request">Account type and currency</param>
    /// <returns>The created account details</returns>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAccountRequest request)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var account = await _accountService.CreateAsync(userId, request.Type, request.Currency);
        return Ok(account);
    }

    /// <summary>
    /// Returns all active accounts belonging to the authenticated user.
    /// </summary>
    /// <returns>List of accounts</returns>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var accounts = await _accountService.GetByUserAsync(userId);
        return Ok(accounts);
    }

    /// <summary>
    /// Searches accounts by recipient name or email (excludes own accounts).
    /// </summary>
    /// <param name="q">Search query (min 2 characters)</param>
    /// <returns>List of matching accounts</returns>
    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string q)
    {
        if (string.IsNullOrWhiteSpace(q) || q.Length < 2)
            return BadRequest(new { message = "Query too short." });

        var currentUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var results = await _accountService.SearchAccountsAsync(q, currentUserId);
        return Ok(results);
    }

    /// <summary>
    /// Returns a specific account by ID.
    /// </summary>
    /// <param name="id">The account ID (GUID)</param>
    /// <returns>Account details</returns>
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