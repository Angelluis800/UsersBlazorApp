using Microsoft.EntityFrameworkCore;
using UsersBlazorApp.API.Context;
using UsersBlazorApp.Data.Interfaces;
using UsersBlazorApp.Data.Models;

namespace UserBlazorApp.API.Services;


public class UserService(UsersDbContext Contexto) : IApiService<AspNetUsers>
{
    public async Task<List<AspNetUsers>> GetAllAsync()
    {
        return await Contexto.AspNetUsers
        .Include(u => u.Role)
        //    .ThenInclude(r => r.AspNetRoleClaims)
        .ToListAsync();
    }

    public async Task<AspNetUsers> GetByIdAsync(int id)
    {
        return (await Contexto.AspNetUsers.FindAsync(id))!;
    }

    public async Task<AspNetUsers> AddAsync(AspNetUsers user)
    {
		Contexto.AspNetUsers.Add(user);

		// Asignar roles existentes al usuario por ID
		if (user.Role != null && user.Role.Any())
		{
			List<AspNetRoles> rolesToAdd = new List<AspNetRoles>();
			foreach (var rol in user.Role)
			{
				var role = await Contexto.AspNetRoles.FindAsync(rol.Id);
				if (role != null)
				{
					rolesToAdd.Add(role);
				}
				else
				{
					// Manejar caso donde el rol no existe (opcional)
					// Puedes lanzar una excepción, retornar un BadRequest, etc.
				}
			}

			// Asignar los roles encontrados al usuario
			user.Role = rolesToAdd;
		}

		await Contexto.SaveChangesAsync();

		return user;
		//Contexto.AspNetUsers.Add(user);

		//foreach (var rol in user.Role)
		//{
		//	var result = await Contexto.AspNetRoles.FindAsync(rol.Id);
		//	if (result != null)
		//	{
		//		rol.Id = result.Id;
		//		rol.Name = result.Name;
		//		// Aquí asignas el rol encontrado al usuario
		//		user.Role = new List<AspNetRoles> { rol };
		//	}
		//}

		//await Contexto.SaveChangesAsync();

		//return user;
	}

    public async Task<bool> UpdateAsync(AspNetUsers user)
    {
        Contexto.Entry(user).State = EntityState.Modified;
        return await Contexto.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var usuario = await Contexto.AspNetUsers.FindAsync(id);
        if (usuario == null)
            return false;

        Contexto.AspNetUsers.Remove(usuario);
        return await Contexto.SaveChangesAsync() > 0;
    }
}
