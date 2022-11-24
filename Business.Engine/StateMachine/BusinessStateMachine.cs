using Automatonymous;
using Business.Event;
using Contracts.Enums;

namespace Business.Engine.StateMachine;

public class BusinessStateMachine  : MassTransitStateMachine<BusinessState>
{
    public State CreateRequested { get; init; } = default!;
    public State GetStateRequested { get; init; } = default!;

    public State WaitRestartStatus { get; init; } = default!;
    public State Error { get; init; } = default!;

    public Event<TheFlow.CreateWork.Raised> Raised { get; init; } = default!;

    public Event<TheFlow.Create.Responded> CreateResponded { get; init; } = default!;

    public Event<TheFlow.GetStatus.Responded> GetStatusResponded { get; init; } = default!;

    public Schedule<BusinessState, TheFlow.CreateWork.RestartGetState> RestartCheckStatus { get; init; } = default!;

    public BusinessStateMachine()
    {
        InstanceState(s => s.CurrentState);

        Event(
            () => Raised,
            x =>
            {
                x.InsertOnInitial = true;

                x.SetSagaFactory(c => new BusinessState
                {
                    CorrelationId = c.Message.CorrelationId,
                    Id = c.Message.Id,
                    CreatedAt = DateTimeOffset.UtcNow,
                });
            });

        Schedule(() => RestartCheckStatus, state => state.RestartTimeoutTokenId, s =>
        {
            s.Delay = TimeSpan.FromSeconds(10);
            s.Received = r => r.CorrelateById(c => c.Message.CorrelationId);
        });

        Initially(When(Raised)
            .Publish(x => new TheFlow.Create.Requested
            {
                Id = x.Instance.Id,
                CorrelationId = x.Instance.CorrelationId
            })
            .TransitionTo(CreateRequested));

        During(CreateRequested, When(CreateResponded)
            .Publish(x => new TheFlow.GetStatus.Requested
            {
                CorrelationId = x.Instance.CorrelationId,
                Id = x.Instance.Id
            })
            .TransitionTo(GetStateRequested));

        During(GetStateRequested,
            When(GetStatusResponded, x => x.Data.Type == StateWorkType.Process)
            .Schedule(RestartCheckStatus, c => new TheFlow.CreateWork.RestartGetState
            {
                CorrelationId = c.Instance.CorrelationId
            }).TransitionTo(WaitRestartStatus),
            When(GetStatusResponded, x => x.Data.Type == StateWorkType.End)
            .Publish(c => new TheFlow.CreateWork.RaisedCompleted
            {
                CorrelationId = c.Instance.CorrelationId
            }).Finalize(),
            When(GetStatusResponded, x => x.Data.Type == StateWorkType.Error)
            .Publish(c => new TheFlow.CreateWork.RaisedCompleted
            {
                CorrelationId = c.Instance.CorrelationId
            }).TransitionTo(Error));

        During(WaitRestartStatus, When(RestartCheckStatus.Received)
        .Publish(x => new TheFlow.GetStatus.Requested
        {
            CorrelationId = x.Instance.CorrelationId,
            Id = x.Instance.Id
        }).TransitionTo(GetStateRequested));
    }
}