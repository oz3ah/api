using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Shortha.Application;
using Shortha.Application.Interfaces.Services;

namespace Shortha.Filters
{
    public class SignedRequestFilter(IAppConnectionService appConnectionService) : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var headers = context.HttpContext.Request.Headers;

            if (!headers.TryGetValue("X-Signature", out var signature) ||
                !headers.TryGetValue("X-Timestamp", out var timestamp))
            {
                throw new BadHttpRequestException("Don't manipulate the request");
            }

            var pairCode = headers["X-Pair-Code"].ToString();

            var user = await appConnectionService.GetByPairCode(pairCode);
            if (user == null || user.IsRevoked())
            {
                throw new BadHttpRequestException("Invalid or revoked Pair Code");
            }

            var secretKey = user.GetSecretKey();

            // --- Read body properly ---
            context.HttpContext.Request.EnableBuffering();
            string body;
            using (var reader = new StreamReader(context.HttpContext.Request.Body, Encoding.UTF8, leaveOpen: true))
            {
                body = await reader.ReadToEndAsync();
                context.HttpContext.Request.Body.Position = 0;
            }

            var hashedBody = await Crypto.GenerateBodyHashAsync(body);

            var httpMethod = context.HttpContext.Request.Method.ToUpperInvariant();
            var urlPath = context.HttpContext.Request.Path.ToString();
            var message = $"{httpMethod}.{urlPath}.{timestamp}.{hashedBody}";

            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey ?? ""));
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));
            var computedSignature = BitConverter.ToString(computedHash).Replace("-", "").ToLowerInvariant();

            if (!string.Equals(computedSignature, signature.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                throw new BadHttpRequestException("Invalid Signature");
            }

            await next(); // continue pipeline
        }
    }
}