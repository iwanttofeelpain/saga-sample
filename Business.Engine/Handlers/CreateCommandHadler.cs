using Business.Engine.DbContexts;
using Business.Engine.Models;
using Contracts.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Business.Engine.Handlers;

public class CreateCommandHadler : IRequestHandler<CreateCommand, bool>
{
    private readonly BusinessContext _context;

    public CreateCommandHadler(BusinessContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(CreateCommand request, CancellationToken cancellationToken)
    {
        var business = await _context.BusinessWorks.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (business != null)
        {
            business.StateWorkType = request.Type;
            _context.Update(business);
            await _context.SaveChangesAsync(cancellationToken);
        }
        return true;
    }
}