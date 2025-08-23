using FluentValidation;
using Shortha.Domain.Enums;

namespace Shortha.Application.Dto.Requests.AppConnections;

public class CreateConnectionDto
{
    public decimal Version { get; set; }
    public ConnectionDevice Device { get; set; }

    public Dictionary<string, object>? DeviceMetadata { get; set; }
}

public class CreateConnectionDtoValidation : AbstractValidator<CreateConnectionDto>
{
    public CreateConnectionDtoValidation()
    {
        RuleFor(x => x.Version)
            .NotNull()
            .GreaterThan(0).WithMessage("Version must be greater than 0");
        RuleFor(x => x.Device).IsInEnum().WithMessage("Invalid device type");

        When(x => x.DeviceMetadata is not null, () =>
        {
            RuleForEach(x => x.DeviceMetadata!).ChildRules(metadata =>
            {
                metadata.RuleFor(x => x.Key)
                    .NotEmpty().WithMessage("Metadata key cannot be empty");
                metadata.RuleFor(x => x.Value)
                    .NotNull().WithMessage("Metadata value cannot be null");
                metadata.RuleFor(x => x.Value)
                    .Must(v => v is not string s || !string.IsNullOrWhiteSpace(s))
                    .WithMessage("Metadata string values cannot be empty");
            });
        });
    }
}