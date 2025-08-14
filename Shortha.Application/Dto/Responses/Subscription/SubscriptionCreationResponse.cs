using Shortha.Domain.Enums;

namespace Shortha.Application.Dto.Responses.Subscription
{
    public class SubscriptionCreationResponse
    {
        public required string Id { get; set; }

        public DateTime StartDate { get; init; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public PackagesName Name { get; init; }
        public decimal Price { get; set; }
        public string? PaymentLink { get; set; }
        public PaymentStatus Status { get; set; }
    }
}