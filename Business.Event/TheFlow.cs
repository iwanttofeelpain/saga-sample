using Contracts.Enums;
using MassTransit;
using MassTransit.Contracts.JobService;

namespace Business.Event;

public static class TheFlow
{
    public static class CreateWork
    {
        public sealed record Raised : CorrelatedBy<Guid>
        {
            public Guid Id { get; set; }
            public Guid CorrelationId { get; set; }
        }

        public sealed record RaisedCompleted : CorrelatedBy<Guid>
        {
            public Guid CorrelationId { get; set; }
        }

        public sealed record RestartGetState : CorrelatedBy<Guid>
        {
            public Guid CorrelationId { get; set; }
        }
    }

    public static class Create
    {
        public sealed record Requested : CorrelatedBy<Guid>
        {
            public Guid Id { get; set; }
            public Guid CorrelationId { get; set; }
        }

        public sealed record Responded : CorrelatedBy<Guid>
        {
            public Guid CorrelationId { get; set; }
        }
    }

    public static class GetStatus
    {
        public sealed record Requested : CorrelatedBy<Guid>
        {
            public Guid Id { get; set; }
            public Guid CorrelationId { get; set; }
        }

        public sealed record Responded : CorrelatedBy<Guid>
        {
            public Guid CorrelationId { get; set; }

            public StateWorkType Type { get; set; }
        }
    }
}