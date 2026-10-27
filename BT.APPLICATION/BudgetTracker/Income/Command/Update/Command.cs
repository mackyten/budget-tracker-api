using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BT.APPLICATION.BudgetTracker.Income.Models;
using BT.DOMAIN.Entities.BudgetTracker;
using BT.PERSISTENCE.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using IncomeData = BT.DOMAIN.Entities.BudgetTracker.Income;

namespace BT.APPLICATION.BudgetTracker.Income.Command.Update
{
    public class Command : IRequest<Response>
    {
        public required IncomeModel Income { get; set; }
    }

    public class CommandHandler : IRequestHandler<Command, Response>
    {
        private readonly BTDbContext _context;

        private readonly IMapper _mapper;

        public CommandHandler(BTDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            try
            {
                var income = await _context.Incomes
                     .Include(x => x.Banks)
                     .Include(x => x.Cash)
                     .Include(x => x.Deductions)
                     .FirstOrDefaultAsync(x => x.Id == request.Income.Id, cancellationToken)
                     ?? throw new Exception("Cannot find item.");

                // Apply the changes to the tracked entity so SaveChanges actually persists them.
                // The child rows have no ids in the model, so they are replaced wholesale.
                income.Date = request.Income.Date;

                _context.RemoveRange(income.Banks);
                _context.RemoveRange(income.Cash);
                _context.RemoveRange(income.Deductions);

                income.Banks = _mapper.Map<List<BankAccount>>(request.Income.Banks);
                income.Cash = _mapper.Map<List<CashOnHand>>(request.Income.Cash);
                income.Deductions = _mapper.Map<List<Deduction>>(request.Income.Deductions);

                await _context.SaveChangesAsync(cancellationToken);

                var response = _mapper.Map<IncomeModel>(income);

                return new SuccessResponse<IncomeModel>(response);
            }
            catch (Exception e)
            {
                return new BadRequestResponse(e.GetBaseException().Message);
            }
            finally
            {
                await _context.DisposeAsync();
            }
        }
    }
}