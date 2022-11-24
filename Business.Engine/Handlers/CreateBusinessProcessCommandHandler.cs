using Business.API.Commands;
using Business.Engine.DbContexts;
using Business.Engine.Entities;
using Business.Event;
using Contracts.Enums;
using MassTransit;
using MediatR;

namespace Business.Engine.Handlers;

public class CreateBusinessProcessCommandHandler : IRequestHandler<CreateBusinessProcessCommand, Guid>
{
    private readonly BusinessContext _context;
    private readonly IPublishEndpoint _endpoint;

    public CreateBusinessProcessCommandHandler(BusinessContext context, IPublishEndpoint endpoint)
    {
        _context = context;
        _endpoint = endpoint;
    }

    public async Task<Guid> Handle(CreateBusinessProcessCommand request, CancellationToken cancellationToken)
    {
        var business = new BusinessWork
        {
            Id = Guid.NewGuid(),
            StateWorkType = StateWorkType.Start
        };
        _context.BusinessWorks.Add(business);
        await _context.SaveChangesAsync(cancellationToken);
        await _endpoint.Publish(new TheFlow.CreateWork.Raised
        {
            CorrelationId = Guid.NewGuid(),
            Id = business.Id
        });
        return business.Id;
    }
}