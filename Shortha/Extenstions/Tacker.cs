using Shortha.Domain;

namespace Shortha.Extenstions
{
    public static class Tacker
    {
        public static Tracker? GetTracker(this HttpContext context)
        {
            var tracker = context.Items["Tracker"] as Tracker;

            return tracker;

        }


    }
}
