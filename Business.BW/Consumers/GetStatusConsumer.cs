using Business.Engine.Models;
using Business.Event;
using MassTransit;
using MediatR;

namespace Business.BW.Consumers;

public class GetStatusConsumer : IConsumer<TheFlow.GetStatus.Requested>
{
    private readonly IMediator _mediator;

    public GetStatusConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<TheFlow.GetStatus.Requested> context)
    {
        var status = await _mediator.Send(new GetStatusQuery
        {
            Id = context.Message.Id
        });
        await context.RespondAsync(new TheFlow.GetStatus.Responded
        {
            CorrelationId = context.Message.CorrelationId,
            Type = status
        });
    }
}