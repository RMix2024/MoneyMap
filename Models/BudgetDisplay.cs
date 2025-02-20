namespace MoneyMap.Models;

public class BudgetDisplay
    {
    public string Category { get; set; } // ✅ Store Category Name from Enum
    public decimal BudgetLimit { get; set; }
    public decimal AmountSpent { get; set; }
    public decimal RemainingAmount { get; set; }
    public bool IsOverBudget { get; set; }
    public double Progress => BudgetLimit > 0 ? (double)AmountSpent / (double)BudgetLimit : 0;
    }
