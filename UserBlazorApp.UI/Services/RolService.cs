using System.Net.Http;
using System.Net.Http.Json;
using UsersBlazorApp.Data.Interfaces;
using UsersBlazorApp.Data.Models;

namespace UserBlazorApp.UI.Services;

public class RolService(HttpClient httpClient) : IClientService<AspNetRoles>
{
	public async Task<List<AspNetRoles>> GetAllAsync()
	{
		return await httpClient.GetFromJsonAsync<List<AspNetRoles>>("api/AspNetRoles");
	}
	public async Task<AspNetRoles> GetByIdAsync(int id)
	{
		return await httpClient.GetFromJsonAsync<AspNetRoles>($"api/AspNetRoles/{id}");
	}
	public async Task<AspNetRoles> AddAsync(AspNetRoles entity)
	{
		var response = await httpClient.PostAsJsonAsync("api/AspNetRoles", entity);
		return await response.Content.ReadFromJsonAsync<AspNetRoles>();
	}

	public async Task<AspNetRoleClaims> AddClaimToRoleAsync(int roleId, AspNetRoleClaims claimRequest)
	{
		var response = await httpClient.PostAsJsonAsync($"api/AspNetRoles/{roleId}/claims", claimRequest);
		response.EnsureSuccessStatusCode();
		return await response.Content.ReadFromJsonAsync<AspNetRoleClaims>();
	}
	public async Task<bool> UpdateAsync(AspNetRoles entity)
	{
		var response = await httpClient.PutAsJsonAsync($"api/AspNetRoles/{entity.Id}", entity);
		return response.IsSuccessStatusCode;
	}

	public async Task<bool> DeleteAsync(int id)
	{
		var response = await httpClient.DeleteAsync($"api/AspNetRoles/{id}");
		return response.IsSuccessStatusCode;
	}

	public async Task<bool> DeleteRoleClaimAsync(int roleId, int claimId)
	{
		var response = await httpClient.DeleteAsync($"api/AspNetRoles/{roleId}/claims/{claimId}");
		return response.IsSuccessStatusCode;
	}



}
