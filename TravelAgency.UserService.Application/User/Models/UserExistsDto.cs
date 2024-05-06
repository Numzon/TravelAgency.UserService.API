namespace TravelAgency.UserService.Application.User.Models;
public class UserExistsDto
{
    public string? UserId { get; set; }
    public bool Exists { get; set; }
}
