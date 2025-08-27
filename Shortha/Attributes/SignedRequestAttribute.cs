using Microsoft.AspNetCore.Mvc;
using Shortha.Filters;

namespace Shortha.Attributes
{
    public class SignedRequestAttribute : TypeFilterAttribute
    {
        public SignedRequestAttribute(bool isRequired = true)
            : base(typeof(SignedRequestFilter))
        {
            Arguments = [isRequired];
        }
    }
}