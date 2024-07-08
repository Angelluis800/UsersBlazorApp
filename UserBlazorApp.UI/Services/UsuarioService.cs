using System.Net.Http;
using System.Net.Http.Json;
using UsersBlazorApp.Data.Interfaces;
using UsersBlazorApp.Data.Models;

namespace UserBlazorApp.UI.Services;

public class UsuarioService(HttpClient httpClient) : IClientService<AspNetUsers>
{
	public async Task<List<AspNetUsers>> GetAllAsync()
	{
		return await httpClient.GetFromJsonAsync<List<AspNetUsers>>("api/AspNetUsers");
	}

	public async Task<AspNetUsers> GetByIdAsync(int id)
	{
		return (await httpClient.GetFromJsonAsync<AspNetUsers>($"api/AspNetUsers/{id}"))!;
	}

	public async Task<AspNetUsers> AddAsync(AspNetUsers entity)
	{
        var response = await httpClient.PostAsJsonAsync("api/AspNetUsers", entity);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<AspNetUsers>();
        }
        else
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            Console.Error.WriteLine($"Error: {errorContent}");
            return null;
        }
    }
	public async Task<bool> UpdateAsync(AspNetUsers entity)
	{
		var response = await httpClient.PutAsJsonAsync($"api/AspNetUsers/{entity.Id}", entity);
		return response.IsSuccessStatusCode;
	}

	public async Task<bool> DeleteAsync(int id)
	{
		var response = await httpClient.DeleteAsync($"api/AspNetUsers/{id}");
		return response.IsSuccessStatusCode;
	}
}
