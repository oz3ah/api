using Shortha.Domain;

namespace Shortha.Extenstions
{
    public static class RequestInfoExtensions
    {
        public static RequestInfo GetRequestInfo(this HttpContext context)
        {
            var requestInfo = context.Items["RequestInfo"] as RequestInfo;
            return requestInfo!;
        }
    }

}
