namespace Shortha.Domain.Dto;

public class UserInfoDto
{
    public string Sub { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public bool EmailVerified { get; set; }
    public string Picture { get; set; } = null!;
}