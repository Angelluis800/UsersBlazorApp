using Microsoft.EntityFrameworkCore;
using UsersBlazorApp.API.Context;
using UsersBlazorApp.Data.Interfaces;
using UsersBlazorApp.Data.Models;

public class RoleService(UsersDbContext Contexto) : IApiService<AspNetRoles>
{
    public async Task<List<AspNetRoles>> GetAllAsync()
    {
        return await Contexto.AspNetRoles
           .Include(r => r.AspNetRoleClaims)
           .ToListAsync();
    }

    public async Task<AspNetRoles> GetByIdAsync(int id)
    {
        return await Contexto.AspNetRoles
            .Include(r => r.AspNetRoleClaims)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<AspNetRoles> AddAsync(AspNetRoles role)
    {
      //  var claims = role.AspNetRoleClaims;     
        var json = System.Text.Json.JsonSerializer.Serialize(role);
        Console.WriteLine("Incoming Role: " + json);

        Contexto.AspNetRoles.Add(role);        
       // await AddClaimToRoleAsync(role.Id, claims);
        await Contexto.SaveChangesAsync();
        return role;
    }
	//public async Task AddClaimToRoleAsync(int roleId, ICollection<AspNetRoleClaims> claimRequests)
	//{
	//	var role = await GetByIdAsync(roleId);
	//	if (role == null)
	//	{
	//		throw new Exception("Role not found");
	//	}
	
	//	foreach (var claimRequest in claimRequests)
	//	{
	//		claimRequest.RoleId = roleId;
	//		role.AspNetRoleClaims.Add(claimRequest);
	//	}

	//	await UpdateAsync(role);
	//}

	public async Task<bool> UpdateAsync(AspNetRoles role)
    {
        Contexto.Entry(role).State = EntityState.Modified;
        return await Contexto.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var role = await Contexto.AspNetRoles.FindAsync(id);
        if (role == null)
            return false;

        Contexto.AspNetRoles.Remove(role);
        return await Contexto.SaveChangesAsync() > 0;
    }
}
