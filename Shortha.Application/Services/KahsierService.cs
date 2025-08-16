using Shortha.Domain.Entites;
using Shortha.Domain.Interfaces;
using System.Security.Cryptography;
using System.Web;

namespace Shortha.Application.Services;

public interface IKahsierService
{
    string Url(string paymentHash, Package package);
}

public class KahsierService(ISecretService secretService) : IKahsierService
{
    private const string BaseUrl = "https://payments.kashier.io/";

    private readonly string _mid = secretService.GetSecret("KashierMID");
    private const string Currency = "EGP";

    private readonly string _secretApi = secretService.GetSecret("KashierSecret");

    private string CreateHash(decimal amount, string orderId)
    {
        var path = $"/?payment={_mid}.{orderId}.{amount}.{Currency}";


        using var hmac = new HMACSHA256(System.Text.Encoding.ASCII.GetBytes(_secretApi));
        var hash = hmac.ComputeHash(System.Text.Encoding.ASCII.GetBytes(path));
        return BitConverter.ToString(hash).Replace("-", "").ToLower();
    }

    public string Url(string paymentHash, Package package)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);

        query["merchantId"] = _mid;
        query["orderId"] = paymentHash;
        query["amount"] = package.Price.ToString();
        query["currency"] = "EGP";
        query["hash"] = CreateHash(package.Price, paymentHash);
        query["mode"] = "test";
        query["interactionSource"] = "Ecommerce";
        query["enable3DS"] = "true";
        query["metaData"] = paymentHash;


        var queryString = query.ToString();

        // Append the URLs manually
        queryString += $"&merchantRedirect={secretService.GetSecret("PaymentCallbackURL")}";
        queryString += "&serverWebhook=https://shortha.gitnasr.com/api/Subscription/payment";

        var uri = $"{BaseUrl}?{queryString}";


        return uri;
    }
}