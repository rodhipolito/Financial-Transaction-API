using FinancialTransactionAPI.Data;
using FinancialTransactionAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FinancialTransactionAPI.Services;

public class AuditLogService
{
    private readonly AppDbContext _db;

    public AuditLogService(AppDbContext db)
    {
        _db = db;
    }

    public async Task LogAsync(string action, string entity, string? entityId = null, string? details = null, Guid? userId = null)
    {
        var log = new AuditLog
        {
            Action = action,
            Entity = entity,
            EntityId = entityId,
            Details = details,
            UserId = userId
        };

        _db.AuditLogs.Add(log);
        await _db.SaveChangesAsync();
    }

    public async Task<List<AuditLog>> GetAllAsync()
    {
        return await _db.AuditLogs
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();
    }
}