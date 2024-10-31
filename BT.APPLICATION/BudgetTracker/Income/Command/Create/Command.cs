using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BT.APPLICATION.BudgetTracker.Income.Models;
using BT.PERSISTENCE.Context;
using MediatR;
using IncomeData = BT.DOMAIN.Entities.BudgetTracker.Income;

namespace BT.APPLICATION.BudgetTracker.Income.Command.Create
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
                var newIncome = _mapper.Map<IncomeData>(request.Income);
                _context.Add(newIncome);
                await _context.SaveChangesAsync(cancellationToken);

                var response = _mapper.Map<IncomeModel>(newIncome);

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