using Shortha.Domain.Interfaces;
using StackExchange.Redis;

namespace Shortha.Infrastructre.Cache;

public class Redis
{
    public class RedisService : IRedisService
    {
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _database;

        public RedisService(ISecretService secretService)
        {
            _redis = ConnectionMultiplexer.Connect(
                                                   new ConfigurationOptions()
                                                   {
                                                       EndPoints =
                                                       {
                                                           {
                                                               secretService.GetSecret("RedisHost"),
                                                               int.Parse(secretService.GetSecret("RedisPort"))
                                                           }
                                                       },
                                                       User = secretService.GetSecret("RedisUsername"),
                                                       Password =
                                                           secretService.GetSecret("RedisPassword")
                                                   }
                                                  );
            _database = _redis.GetDatabase();
        }

        public async Task SetValue(string key, string value)
        {
            await _database.StringSetAsync(key, value);
        }

        public async Task SetValue(string key, string value, TimeSpan expireTime)
        {
            await _database.StringSetAsync(key, value, expireTime);
        }

        public string? GetValue(string key)
        {
            var value = _database.StringGet(key);
            return value.IsNullOrEmpty ? null : value.ToString();
        }
    }
}