using Contracts.Enums;
using MediatR;

namespace Business.Engine.Models;

public class GetStatusQuery : IRequest<StateWorkType>
{
    public Guid Id { get; set; }
}