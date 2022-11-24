using Automatonymous;
using MassTransit.Saga;

namespace Business.Engine.StateMachine;

public class BusinessState : SagaStateMachineInstance, ISagaVersion
{
    public Guid CorrelationId { get; set; }

    public string CurrentState { get; set; } = default!;

    public Guid Id { get; set; }

    public Guid? RestartTimeoutTokenId { get; set; }

    public DateTimeOffset CreatedAt { get;  set; }
    public int Version { get; set; }
}