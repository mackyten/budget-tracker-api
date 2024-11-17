using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BT.APPLICATION.BudgetTracker.Income.Models;
using BT.DOMAIN.Entities.BudgetTracker;
using BT.PERSISTENCE.Context;
using MediatR;
using IncomeData = BT.DOMAIN.Entities.BudgetTracker.Income;

namespace BT.APPLICATION.BudgetTracker.Income.Command.Create
{
    public class Command : IRequest<Response>
    {
        public DateTime Date { get; set; }
        public List<BankAccountModel> Banks { get; set; } = [];
        public List<CashOnHandModel> Cash { get; set; } = [];
        public List<DeductionModel> Deductions { get; set; } = [];
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
                var newData = new IncomeData
                {
                    Date = request.Date,
                    Banks = _mapper.Map<List<BankAccount>>(request.Banks),
                    Cash = _mapper.Map<List<CashOnHand>>(request.Cash),
                    Deductions = _mapper.Map<List<Deduction>>(request.Deductions)
                };

                _context.Add(newData);
                await _context.SaveChangesAsync(cancellationToken);

                var response = _mapper.Map<IncomeModel>(newData);

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