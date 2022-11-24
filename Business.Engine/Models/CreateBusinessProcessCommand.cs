using MediatR;

namespace Business.API.Commands;

public record CreateBusinessProcessCommand : IRequest<Guid>
{

}