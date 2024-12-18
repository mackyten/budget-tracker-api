using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BT.DOMAIN.Entities.BudgetTracker;
using Microsoft.EntityFrameworkCore;

namespace BT.PERSISTENCE.Context
{
    public partial class BTDbContext
    {
        public DbSet<Income> Incomes { get; set; }
        public DbSet<Deduction> Deductions { get; set; }
        public DbSet<CashOnHand> CashOnHands { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
    }
}