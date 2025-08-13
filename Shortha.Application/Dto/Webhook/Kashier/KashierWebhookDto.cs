using System.Text.Json.Serialization;

namespace Shortha.Application.Dto.Webhook.Kashier;

public class KashierWebhookDto
{
    [JsonPropertyName("event")] public string Event { get; set; }

    [JsonPropertyName("data")] public PaymentDataDto Data { get; set; }

    [JsonPropertyName("hash")] public string Hash { get; set; }
}

public class PaymentDataDto
{
    [JsonPropertyName("merchantOrderId")] public string MerchantOrderId { get; set; }

    [JsonPropertyName("kashierOrderId")] public string KashierOrderId { get; set; }

    [JsonPropertyName("orderReference")] public string OrderReference { get; set; }

    [JsonPropertyName("transactionId")] public string TransactionId { get; set; }

    [JsonPropertyName("status")] public string Status { get; set; }

    [JsonPropertyName("method")] public string Method { get; set; }

    [JsonPropertyName("creationDate")] public DateTime CreationDate { get; set; }

    [JsonPropertyName("amount")] public decimal Amount { get; set; }

    [JsonPropertyName("currency")] public string Currency { get; set; }

    [JsonPropertyName("card")] public CardDto Card { get; set; }

    [JsonPropertyName("metaData")] public MetaDataDto MetaData { get; set; }

    [JsonPropertyName("sourceOfFunds")] public SourceOfFundsDto SourceOfFunds { get; set; }

    [JsonPropertyName("transactionResponseCode")]
    public string TransactionResponseCode { get; set; }

    [JsonPropertyName("transactionResponseMessage")]
    public TransactionResponseMessageDto TransactionResponseMessage { get; set; }

    [JsonPropertyName("channel")] public string Channel { get; set; }

    [JsonPropertyName("merchantDetails")] public MerchantDetailsDto MerchantDetails { get; set; }


    [JsonPropertyName("signatureKeys")] public List<string> SignatureKeys { get; set; }
}

public class CardDto
{
    [JsonPropertyName("cardInfo")] public CardInfoDto CardInfo { get; set; }

    [JsonPropertyName("merchant")] public MerchantDto Merchant { get; set; }

    [JsonPropertyName("amount")] public decimal Amount { get; set; }

    [JsonPropertyName("currency")] public string Currency { get; set; }
}

public class CardInfoDto
{
    [JsonPropertyName("cardHolderName")] public string CardHolderName { get; set; }

    [JsonPropertyName("cardBrand")] public string CardBrand { get; set; }

    [JsonPropertyName("maskedCard")] public string MaskedCard { get; set; }
}

public class MerchantDto
{
    [JsonPropertyName("merchantRedirectURL")]
    public string MerchantRedirectUrl { get; set; }
}

public class MetaDataDto
{
    [JsonPropertyName("kashier payment UI version")]
    public string KashierPaymentUiVersion { get; set; }

    [JsonPropertyName("termsAndConditions")]
    public TermsAndConditionsDto TermsAndConditions { get; set; }
}

public class TermsAndConditionsDto
{
    [JsonPropertyName("ip")] public string Ip { get; set; }
}

public class SourceOfFundsDto
{
    [JsonPropertyName("cardInfo")] public DetailedCardInfoDto CardInfo { get; set; }
}

public class DetailedCardInfoDto
{
    [JsonPropertyName("maskedCard")] public string MaskedCard { get; set; }

    [JsonPropertyName("cardBrand")] public string CardBrand { get; set; }

    [JsonPropertyName("cardHolderName")] public string CardHolderName { get; set; }

    [JsonPropertyName("cardDataToken")] public string CardDataToken { get; set; }

    [JsonPropertyName("ccvToken")] public string CcvToken { get; set; }

    [JsonPropertyName("expiryYear")] public string ExpiryYear { get; set; }

    [JsonPropertyName("expiryMonth")] public string ExpiryMonth { get; set; }

    [JsonPropertyName("storedOnFile")] public string StoredOnFile { get; set; }

    [JsonPropertyName("save")] public bool Save { get; set; }
}

public class TransactionResponseMessageDto
{
    [JsonPropertyName("en")] public string En { get; set; }

    [JsonPropertyName("ar")] public string Ar { get; set; }
}

public class MerchantDetailsDto
{
    [JsonPropertyName("businessEmail")] public string BusinessEmail { get; set; }
}