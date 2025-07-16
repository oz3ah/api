namespace Shortha.Domain.Interfaces
{
    public interface IRedisService
    {
        Task SetValue(string key, string value);
        Task SetValue(string key, string value, TimeSpan expireTime);
        string? GetValue(string key);
    }
}