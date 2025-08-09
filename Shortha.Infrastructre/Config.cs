namespace Shortha.Infrastructre;

public static class Config 
{
    public static readonly string AppId = Environment.GetEnvironmentVariable("APP_ID") ?? "shortha-gitnasr";
    public static readonly string AppName = Environment.GetEnvironmentVariable("APP_NAME") ?? "shortha";
    public static readonly string Env = Environment.GetEnvironmentVariable("ENVIRONMENT") ?? "development";

}