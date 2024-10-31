using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BT.DOMAIN.Entities.BudgetTracker;
using BT.PERSISTENCE.Security;
using Microsoft.EntityFrameworkCore;

namespace BT.PERSISTENCE.Context
{
    public class BTDbContext : BaseDbContext
    {
        public BTDbContext(DbContextOptions<BTDbContext> options, IdentityHelper identityHelper)
            : base(options, identityHelper)
        {
        }


        /// ADD ENTITIES HERE
        public DbSet<Income> Incomes { get; set; }
        public DbSet<Deduction> Deductions { get; set; }
        public DbSet<CashOnHand> CashOnHands { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }

    }
}