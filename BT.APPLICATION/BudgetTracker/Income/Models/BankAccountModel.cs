using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BT.APPLICATION.BudgetTracker.Income.Models
{
    public class BankAccountModel
    {
        public required string Name { get; set;} 
        public int Balance { get; set; }
        
    }
}