using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Shortha.Application;
using Shortha.Application.Interfaces.Services;

namespace Shortha.Filters
{
    public class SignedRequestFilter(IAppConnectionService appConnectionService, bool isRequired) : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var headers = context.HttpContext.Request.Headers;

            if (!isRequired && !headers.ContainsKey("X-Signature"))
            {
                await next();
                return;
            }


            if (!headers.TryGetValue("X-Signature", out var signature) ||
                !headers.TryGetValue("X-Timestamp", out var timestamp))
            {
                throw new BadHttpRequestException("Don't manipulate the request");
            }

            if (!long.TryParse(timestamp, out var ts))
                throw new BadHttpRequestException("Invalid timestamp");

            var requestTime = DateTimeOffset.FromUnixTimeSeconds(ts);
            if (DateTimeOffset.UtcNow - requestTime > TimeSpan.FromMinutes(1))
                throw new BadHttpRequestException("Expired signature");

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
            context.HttpContext.Request.Body.Seek(0, SeekOrigin.Begin);

            using (var reader = new StreamReader(
                       context.HttpContext.Request.Body,
                       encoding: Encoding.UTF8,
                       detectEncodingFromByteOrderMarks: false,
                       bufferSize: 1024,
                       leaveOpen: true))
            {
                body = await reader.ReadToEndAsync();
                context.HttpContext.Request.Body.Seek(0, SeekOrigin.Begin);
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

            if (user.UserId != null)
            {
                context.HttpContext.Items["AuthSource"] = "AppConnection";
                var claims = new List<Claim>
                {
                    new(ClaimTypes.NameIdentifier, user.UserId),
                    new("permissions", "create:url")
                };
                var identity = new ClaimsIdentity(claims, "SignedRequest");
                context.HttpContext.User = new ClaimsPrincipal(identity);
            }

            await next(); // continue pipeline
        }
    }
}