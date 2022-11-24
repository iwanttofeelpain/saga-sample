using Contracts.Enums;
using MediatR;

namespace Business.Engine.Models;

public class CreateCommand : IRequest<bool>
{
    public StateWorkType Type { get; set; }
    public Guid Id { get; set; }
}