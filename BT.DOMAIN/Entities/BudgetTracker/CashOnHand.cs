using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BT.DOMAIN.Enums.BudgetTracker;

namespace BT.DOMAIN.Entities.BudgetTracker
{
    public class CashOnHand : BaseEntity<int>
    {
        public int IncomeId { get; set; }
        public Denominations Denomination { get; set; }
        public int Pieces { get; set; }
    }
}