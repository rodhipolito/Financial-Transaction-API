namespace FinancialTransactionAPI.Models;

public enum TransactionType { Deposit, Withdrawal, Transfer }
public enum TransactionStatus { Pending, Completed, Failed }

public class Transaction
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public TransactionType Type { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "EUR";
    public string Description { get; set; } = string.Empty;
    public TransactionStatus Status { get; set; } = TransactionStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ProcessedAt { get; set; }
    public string? Reference { get; set; }

    public Guid AccountId { get; set; }
    public Account Account { get; set; } = null!;

    public Guid? DestinationAccountId { get; set; }
    public Account? DestinationAccount { get; set; }
}