using Shortha.Domain.Entites;
using Shortha.Domain.Interfaces;
using System.Security.Cryptography;
using System.Web;

namespace Shortha.Application.Services;

public interface IKahsierService
{
    string Url(string subscriptionId, Package package);
}

public class KahsierService(ISecretService secretService) : IKahsierService
{
    private const string BaseUrl = "https://payments.kashier.io/";

    private readonly string Mid = secretService.GetSecret("KashierMID");
    private const string Currency = "EGP";

    private readonly string SecretApi = secretService.GetSecret("KashierSecret");

    private string CreateHash(decimal amount, string orderId)
    {
        var path = $"/?payment={Mid}.{orderId}.{amount}.{Currency}";


        using var hmac = new HMACSHA256(System.Text.Encoding.ASCII.GetBytes(SecretApi));
        var hash = hmac.ComputeHash(System.Text.Encoding.ASCII.GetBytes(path));
        return BitConverter.ToString(hash).Replace("-", "").ToLower();
    }

    public string Url(string subscriptionId, Package package)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);

        query["merchantId"] = Mid;
        query["orderId"] = subscriptionId;
        query["amount"] = package.Price.ToString();
        query["currency"] = "EGP";
        query["hash"] = CreateHash(package.Price, subscriptionId);
        query["mode"] = "test";
        query["interactionSource"] = "Ecommerce";
        query["enable3DS"] = "true";
        query["metaData"] = subscriptionId;


        var queryString = query.ToString();

        // Append the URLs manually
        queryString += $"&merchantRedirect=https://shortha.vercel.app/upgraded?orderId={subscriptionId}";
        queryString += "&serverWebhook=https://shortha.gitnasr.com/api/Webhook";

        var uri = $"https://payments.kashier.io/?{queryString}";


        return uri;
    }
}