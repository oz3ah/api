using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shortha.Application.Dto.Webhook.Kashier;
using Shortha.Application.Services;
using Shortha.Extenstions;

namespace Shortha.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SubscriptionController(
    ISubscriptionService subscription,
    IUserService userService,
    ILogger<SubscriptionController> logger) : Base
{
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> SubscribeToPackage([FromBody] SubscribeDto packageDto)
    {
        var userId = User.GetUserId();
        var result = await subscription.Subscribe(userId, packageDto.packageId);

        return Success(result);
    }

    [HttpPost("payment")]
    public async Task<IActionResult> ReceiveWebhookAsync([FromBody] KashierWebhookDto? webhookData)
    {
        if (webhookData == null)
        {
            logger.LogError("Received null webhook data.");
            return Fail("No Webhook Data");
        }

        if (webhookData.Event != "pay") return Ok(new { message = "Webhook received successfully", webhookData });

        logger.LogInformation("Webhook : {KashierWebhookDto}", webhookData);

        var updated = await subscription.UpgradeSubscription(webhookData.Data.MerchantOrderId,
                                                             webhookData.Data.TransactionId,
                                                             webhookData.Data.Method, webhookData.Data.Currency);

        await userService.ChangeSubscriptionType(updated.UserId, true);
        await userService.AlternateUserRole("Pro", updated.UserId);

        return Success("Done");
    }
}