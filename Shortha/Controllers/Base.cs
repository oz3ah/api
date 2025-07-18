using Microsoft.AspNetCore.Mvc;
using Shortha.Domain.Dto;

namespace Shortha.Controllers;

[ApiController]
public  abstract class Base : ControllerBase
{
    protected IActionResult Success<T>(T data, string? message = null)
        => Ok(ApiResponse<T>.Ok(data, message));

    protected IActionResult Fail(string message, List<string>? errors = null)
        => BadRequest(ErrorResponse.From(message, errors, HttpContext.TraceIdentifier));
}