using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BT.DOMAIN.Entities;

namespace BT.APPLICATION.BudgetTracker.Income.Models
{
    public class DeductionModel
    {
        public required string Description { get; set; }
        public int Amount { get; set; }
    }
}