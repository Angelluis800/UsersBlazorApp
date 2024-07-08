namespace UserBlazorApp.API.Dtos.Roles;
using UserBlazorApp.API.Dtos.Claims;

public class RolRequest
{
	public int RoleId { get; set; }
	public string? Name { get; set; }

	public List<RolClaimRequest> Claims { get; set; } = new List<RolClaimRequest>();
}
