using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Shortha.Application.Interfaces.Services;
using Shortha.Filters;

namespace Shortha.Attributes
{
    public class SignedRequestAttribute() : TypeFilterAttribute(typeof(SignedRequestFilter));
}