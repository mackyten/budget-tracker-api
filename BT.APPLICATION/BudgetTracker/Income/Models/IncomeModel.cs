using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BT.APPLICATION.BudgetTracker.Income.Models
{
    public class IncomeModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public List<BankAccountModel> Banks { get; set; } = [];
        public List<CashOnHandModel> Cash { get; set; } = [];
        public List<DeductionModel> Deductions { get; set; } = [];
    }
}