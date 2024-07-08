using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserBlazorApp.API.Dtos.Claims;
using UserBlazorApp.API.Dtos.Roles;
using UsersBlazorApp.API.Context;
using UsersBlazorApp.Data.Interfaces;
using UsersBlazorApp.Data.Models;

namespace UserBlazorApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AspNetRolesController(IApiService<AspNetRoles> roleService) : ControllerBase
    {
		// GET: api/AspNetRoles
		[HttpGet]
		public async Task<ActionResult<IEnumerable<RolResponse>>> GetAspNetRoles()
		{
			var roles = await roleService.GetAllAsync();
			var rolResponse = roles.Select(r => new RolResponse
			{
				Id = r.Id,
				Name = r.Name,
				RoleClaims = r.AspNetRoleClaims.Select(rc => new RolClaimResponse
				{
					Id = rc.Id,
					RoleId = rc.RoleId,
					ClaimType = rc.ClaimType,
					ClaimValue = rc.ClaimValue
				}).ToList()
			}).ToList();

			return Ok(rolResponse);
		}

		// POST: api/AspNetRoles

		[HttpPost]
		public async Task<ActionResult<RolResponse>> PostAspNetRoles(RolRequest rolRequest)
		{
			var rol = new AspNetRoles
			{
				Name = rolRequest.Name,

				AspNetRoleClaims = rolRequest.Claims.Select(c => new AspNetRoleClaims
				{
					ClaimType = c.ClaimType,
					ClaimValue = c.ClaimValue,
					RoleId = c.RoleId
				}).ToList()
			};

			var roleCreated = await roleService.AddAsync(rol);

			var rolResponse = new RolResponse
			{
				Id = roleCreated.Id,
				Name = roleCreated.Name,

				RoleClaims = roleCreated.AspNetRoleClaims.Select(rc => new RolClaimResponse
				{
					Id = rc.Id,
					RoleId = rc.RoleId,
					ClaimType = rc.ClaimType,
					ClaimValue = rc.ClaimValue
				}).ToList()
			};

			return Ok(rolResponse);
			//return CreatedAtAction("GetAspNetRoles", new { id = aspNetRoles.Id }, aspNetRoles);
		}

		// POST: api/AspNetRoles/{roleId}/claims
		[HttpPost("{roleId}/claims")]
		public async Task<IActionResult> AddClaimToRole(int roleId, AspNetRoleClaims claimRequest)
		{
			var role = await roleService.GetByIdAsync(roleId);
			if (role == null)
			{
				return NotFound();
			}

			var newClaim = new AspNetRoleClaims
			{
				RoleId = roleId,
				ClaimType = claimRequest.ClaimType,
				ClaimValue = claimRequest.ClaimValue
			};

			role.AspNetRoleClaims.Add(newClaim);
			await roleService.UpdateAsync(role);

			return Ok(newClaim); // Devolvemos directamente el objeto creado
		}

		// GET: api/AspNetRoles/5
		[HttpGet("{id}")]
		public async Task<ActionResult<RolResponse>> GetAspNetRoles(int id)
		{
			var rol = await roleService.GetByIdAsync(id);

			if (rol == null)
			{
				return NotFound();
			}

			var rolResponse = new RolResponse
			{
				Id = rol.Id,
				Name = rol.Name,
				RoleClaims = rol.AspNetRoleClaims.Select(rc => new RolClaimResponse
				{
					Id = rc.Id,
					RoleId = rc.RoleId,
					ClaimType = rc.ClaimType,
					ClaimValue = rc.ClaimValue
				}).ToList()
			};

			return Ok(rolResponse);
		}

		// PUT: api/AspNetRoles/5
		[HttpPut("{id}")]
		public async Task<IActionResult> PutAspNetRoles(int id, RolRequest rolRequest)
		{
			var rol = await roleService.GetByIdAsync(id);

			if (rol == null)
				return NotFound();

			rol.Name = rolRequest.Name;

			// Obtener los claims actuales del rol
			var existingClaims = rol.AspNetRoleClaims.ToList();

			// Actualizar claims existentes y agregar nuevos claims
			foreach (var requestClaim in rolRequest.Claims)
			{
				var existingClaim = existingClaims.FirstOrDefault(c => c.ClaimType == requestClaim.ClaimType);
				if (existingClaim != null)
				{
					// Actualizar claim existente
					existingClaim.ClaimValue = requestClaim.ClaimValue;
				}
				else
				{
					// Agregar nuevo claim
					rol.AspNetRoleClaims.Add(new AspNetRoleClaims
					{
						RoleId = id,
						ClaimType = requestClaim.ClaimType,
						ClaimValue = requestClaim.ClaimValue
					});
				}

			}
			await roleService.UpdateAsync(rol);

			return NoContent();
		}

		[HttpDelete("{roleId}/claims/{claimId}")]
		public async Task<IActionResult> DeleteClaim(int roleId, int claimId)
		{
			var rol = await roleService.GetByIdAsync(roleId);

			if (rol == null)
				return NotFound();

			var claim = rol.AspNetRoleClaims.FirstOrDefault(c => c.Id == claimId);
			if (claim == null)
				return NotFound();

			rol.AspNetRoleClaims.Remove(claim);
			await roleService.UpdateAsync(rol);

			return NoContent();
		}

		// DELETE: api/AspNetRoles/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteAspNetRoles(int id)
		{
			var rol = await roleService.GetByIdAsync(id);

			if (rol == null)
			{
				return NotFound();
			}

			await roleService.DeleteAsync(id);

			return NoContent();
		}


	}
}
