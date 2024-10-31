using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BT.DOMAIN.Entities.BudgetTracker
{
    public class Deduction : BaseEntity<int>
    {
        public int IncomeId { get; set; }
        public required string Description { get; set; }
        public int Amount { get; set; }
    }
}