using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BT.DOMAIN.Entities.BudgetTracker
{
    public class Income : BaseEntity<int>
    {
        public DateTime Date { get; set; }
        public List<BankAccount> Banks { get; set; } = [];
        public List<CashOnHand> Cash { get; set; } = [];
        public List<Deduction> Deductions { get; set; } = [];
    }
}