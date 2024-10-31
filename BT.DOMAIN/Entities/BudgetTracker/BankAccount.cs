using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BT.DOMAIN.Entities.BudgetTracker
{
    public class BankAccount : BaseEntity<int>
    {
        public int IncomeId { get; set; }
        public required string Name { get; set; }
        public int Balance { get; set; }
    }
}