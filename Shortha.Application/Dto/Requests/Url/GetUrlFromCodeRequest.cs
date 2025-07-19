using FluentValidation;

namespace Shortha.Application.Dto.Requests.Url;

public class GetUrlFromCodeRequest
{
    public string Hash { get; set; }

    public string Fingerprint { get; set; } = string.Empty;
}

public class GetUrlFromHashRequestValidator : AbstractValidator<GetUrlFromCodeRequest>
{
    public GetUrlFromHashRequestValidator()
    {
        RuleFor(x => x.Hash)
            .NotEmpty()
            .WithMessage("Hash cannot be empty.")
            .Length(3, 10)
            .WithMessage("Hash length must be between 3 and 10 characters.");

        RuleFor(x => x.Fingerprint)
            .NotEmpty()
            .WithMessage("Fingerprint cannot be empty.");
    }
}