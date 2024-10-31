using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BT.APPLICATION.BudgetTracker.Income.Models;
using BT.APPLICATION.RequestResponse;
using BT.PERSISTENCE.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BT.APPLICATION.BudgetTracker.Income.Queries.GetList
{
    public class Query : IRequest<Response>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class QueryHandler : IRequestHandler<Query, Response>
    {
        private readonly BTDbContext _context;
        private readonly IMapper _mapper;

        public QueryHandler(BTDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            try
            {
                // Calculate the total count of items for pagination metadata
                var totalCount = await _context.Incomes.CountAsync(cancellationToken);

                // Apply pagination with Skip and Take
                var items = await _context.Incomes
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync(cancellationToken);

                var response = _mapper.Map<List<IncomeModel>>(items);

                // Prepare the paginated response
                var paginatedResponse = new PaginatedResponse<List<IncomeModel>>(response, request.PageNumber, request.PageSize, totalCount);

                return new SuccessResponse<PaginatedResponse<List<IncomeModel>>>(paginatedResponse);
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