using FluentValidation;
using Shortha.Domain.Enums;

namespace Shortha.Application.Dto.Requests.AppConnections;

public class CreateConnectionDto
{
    public required string Version { get; set; }
    public ConnectionDevice Device { get; set; }

    public Dictionary<string, object>? DeviceMetadata { get; set; }
}

public class CreateConnectionDtoValidation : AbstractValidator<CreateConnectionDto>
{
    public CreateConnectionDtoValidation()
    {
        RuleFor(x => x.Version)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.Device).IsInEnum().WithMessage("Invalid device type");

        When(x => x.DeviceMetadata is not null, () =>
        {
            RuleForEach(x => x.DeviceMetadata!).ChildRules(metadata =>
            {
                metadata.RuleFor(x => x.Key)
                    .NotEmpty().WithMessage("Metadata key cannot be empty");
            });
        });
    }
}