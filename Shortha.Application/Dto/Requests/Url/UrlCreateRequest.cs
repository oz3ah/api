using FluentValidation;

namespace Shortha.Application.Dto.Requests.Url
{
    public class UrlCreateRequest
    {
        public required string Url { get; set; }
        public string? CustomHash { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }

    public class UrlCreateRequestValidator : AbstractValidator<UrlCreateRequest>
    {
        public UrlCreateRequestValidator()
        {
            RuleFor(x => x.Url)
                .NotEmpty().WithMessage("URL cannot be empty.")
                .Must(BeAValidUrl).WithMessage("Invalid URL format.");

            // Optional: custom hash validation
            RuleFor(x => x.CustomHash)
                .Matches("^[a-zA-Z0-9_-]{3,30}$")
                .When(x => !string.IsNullOrWhiteSpace(x.CustomHash))
                .WithMessage("Custom hash must be alphanumeric and 3–30 characters.");

            // Optional: expiresAt in the future
            RuleFor(x => x.ExpiresAt)
                .GreaterThan(DateTime.UtcNow)
                .When(x => x.ExpiresAt.HasValue)
                .WithMessage("Expiration date must be in the future.");
        }

        private bool BeAValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out var uriResult) &&
                   (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }
}