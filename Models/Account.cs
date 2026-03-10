namespace FinancialTransactionAPI.Models;

public enum AccountType { Checking, Savings, Investment }

public class Account
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public AccountType Type { get; set; }
    public decimal Balance { get; set; } = 0;
    public string Currency { get; set; } = "EUR";
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}