using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BT.DOMAIN.Enums.BudgetTracker;

namespace BT.APPLICATION.BudgetTracker.Income.Models
{
    public class CashOnHandModel
    {
        public Denominations Denomination { get; set; }
        public int Pieces { get; set; }
    }
}