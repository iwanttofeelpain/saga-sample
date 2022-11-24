using Business.Engine.DbContexts;
using Business.Engine.Models;
using Contracts.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Business.Engine.Handlers;

public class GetStatusQueryHadler : IRequestHandler<GetStatusQuery, StateWorkType>
{
    private readonly BusinessContext _context;

    public GetStatusQueryHadler(BusinessContext context)
    {
        _context = context;
    }

    public async Task<StateWorkType> Handle(GetStatusQuery request, CancellationToken cancellationToken)
    {
        var business = await _context.BusinessWorks.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        return business.StateWorkType;
    }
}