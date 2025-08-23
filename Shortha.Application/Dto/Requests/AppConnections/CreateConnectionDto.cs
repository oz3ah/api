using FluentValidation;
using Shortha.Domain.Enums;

namespace Shortha.Application.Dto.Requests.AppConnections;

public class CreateConnectionDto
{
    public decimal Version { get; set; }
    public ConnectionDevice Device { get; set; }
}

public class CreateConnectionDtoValidation : AbstractValidator<CreateConnectionDto>
{
    public CreateConnectionDtoValidation()
    {
        RuleFor(x => x.Version)
            .NotNull()
            .GreaterThan(0).WithMessage("Version must be greater than 0");
        RuleFor(x => x.Device).IsInEnum().WithMessage("Invalid device type");
    }
}