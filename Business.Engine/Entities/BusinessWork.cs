using Contracts.Enums;

namespace Business.Engine.Entities;

public class BusinessWork
{
    public Guid Id { get; set; }
    public StateWorkType StateWorkType { get; set; }
}