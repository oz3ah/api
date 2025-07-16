using Infisical.Sdk;

namespace Shortha.Infrastructre.Secrets
{
    internal class GetSecret : GetSecretOptions
    {
        public GetSecret()
        {
            Environment = System.Environment.GetEnvironmentVariable("Env") ?? "development";
            ProjectId = System.Environment.GetEnvironmentVariable("ProjectId") ??
                        throw new ArgumentNullException("Can't find the project id");
            Console.WriteLine($"Using the {Environment}");
        }
    }
}
