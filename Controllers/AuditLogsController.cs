using FinancialTransactionAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinancialTransactionAPI.Controllers;

/// <summary>
/// Provides access to the audit log of all system operations. Admin only.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class AuditLogsController : ControllerBase
{
    private readonly AuditLogService _auditLogService;

    public AuditLogsController(AuditLogService auditLogService)
    {
        _auditLogService = auditLogService;
    }

    /// <summary>
    /// Returns all audit log entries ordered by date descending.
    /// </summary>
    /// <remarks>
    /// This endpoint is restricted to Admin users only.
    /// Logs are automatically created for every deposit, withdrawal and transfer.
    /// </remarks>
    /// <returns>List of audit log entries</returns>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var logs = await _auditLogService.GetAllAsync();
        return Ok(logs);
    }
}
