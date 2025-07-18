using FluentValidation;

namespace Shortha.Application.Dto.Requests.Url
{
    public class UrlCreateRequest
    {
        public required string Url { get; set; }

    }
    public class UrlCreateRequestValidator : AbstractValidator<UrlCreateRequest>
    {
        public UrlCreateRequestValidator()
        {
            RuleFor(x => x.Url)
                .NotEmpty().WithMessage("URL cannot be empty.")
                .Must(BeAValidUrl).WithMessage("Invalid URL format.");
        }

        private bool BeAValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out var uriResult) &&
                   (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }
}
