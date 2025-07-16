using Infisical.Sdk;
using System.Collections.Concurrent;

namespace Shortha.Infrastructre.Secrets
{
    internal class Manager
    {

        public class SecretService(InfisicalClient infisicalClient) : ISecretService
        {
            private readonly ConcurrentDictionary<string, string> _cache = new();

            private GetSecret CreateSecret(string name)
            {
                return new GetSecret
                {
                    SecretName = name
                };
            }


            public string GetSecret(string name)
            {
                try
                {
                    if (_cache.TryGetValue(name, out var value))
                        return value;
                    value = infisicalClient.GetSecret(CreateSecret(name)).SecretValue;
                    _cache[name] = value;
                    return value;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error retrieving secret: {ex.Message}");
                    throw;
                }
            }
        }

        public interface ISecretService
        {
            string GetSecret(string name);
        }
    }
}
