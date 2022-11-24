using Business.Engine.Models;
using Business.Event;
using Contracts.Enums;
using MassTransit;
using MediatR;

namespace Business.BW.Consumers;

public class CreateConsumer : IConsumer<TheFlow.Create.Requested>
{
    private readonly IMediator _mediator;

    public CreateConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<TheFlow.Create.Requested> context)
    {
        await _mediator.Send(new CreateCommand
        {
            Id = context.Message.Id,
            Type = StateWorkType.Process
        });
        await context.RespondAsync(new TheFlow.Create.Responded
        {
            CorrelationId = context.Message.CorrelationId
        });
    }
}