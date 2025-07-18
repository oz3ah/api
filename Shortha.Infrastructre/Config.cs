namespace Shortha.Infrastructre;

public static class Config 
{
    public static readonly string appId = Environment.GetEnvironmentVariable("APP_ID") ?? "shortha-gitnasr";
    public static readonly string appName = Environment.GetEnvironmentVariable("APP_NAME") ?? "shortha";
    public static readonly string env = Environment.GetEnvironmentVariable("ENVIRONMENT") ?? "development";

}