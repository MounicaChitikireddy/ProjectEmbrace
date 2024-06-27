using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager) 
        {
            this.roleManager = roleManager;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            if (roleName == null)
            {
                return Problem("Entity set 'webapiContext.roleName'  is null.");
            }
            IdentityRole role = new IdentityRole
            {
                Name = roleName
            };

            IdentityResult result = await roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                return Ok("Role created successfully.");
            }
            return BadRequest();
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Reviewer")]
        public IQueryable<IdentityRole> GetRoles()
        {
            var roles = roleManager.Roles;
            return roles;
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRole(string roleName)
        {
            if (roleManager.Roles.Any(x => x.Name == roleName))
            {
                var role = await roleManager.Roles.SingleAsync(x => x.Name == roleName);
                IdentityResult result = await roleManager.DeleteAsync(role);
                return Ok("Role deleteed successfully.");
            }
            else
            {
                return NotFound();
            }
        }
    }
}
