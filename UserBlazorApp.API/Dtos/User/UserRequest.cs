using UserBlazorApp.API.Dtos.Claims;
using UserBlazorApp.API.Dtos.Roles;
using UsersBlazorApp.Data.Models;

namespace UserBlazorApp.API.Dtos.User;

public class UserRequest
{
    //public int Id { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? PasswordHash { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTimeOffset? LockoutEnd { get; set; }
	public ICollection<RolRequest> Role { get; set; } = new List<RolRequest>();
}
