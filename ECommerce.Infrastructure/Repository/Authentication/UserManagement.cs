using ECommerce.Infrastructure.Data;
using ECommerceApp.Domain.Entities.Identity;
using ECommerceApp.Domain.Interface.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ECommerce.Infrastructure.Repository.Authentication
{
    public class UserManagement (IRoleManagement role,UserManager<AppUser> userManager,AppDbContext context ): IUserManagement
    {
        public async Task<bool> CreateUser(AppUser user)
        {
            var _user = await GetUserByEmail(user.Email!);
            if (_user != null)
            {
                return false;   
            }
            return (await userManager.CreateAsync(user!, user!.PasswordHash!)).Succeeded;
        }

        public async Task<IEnumerable<AppUser>?> GetAllUsers() => await context.Users.ToListAsync();
       

        public async Task<AppUser> GetUserByEmail(string email) =>
        await userManager.FindByEmailAsync(email);

        public async Task<List<Claim>> GetUserClaim(string email)
        {
            var _user=await GetUserByEmail(email);
            string? roleName=await role.GetUserRole(_user!.Email!);
            List<Claim> claims = new()
        {
            new Claim("FullName", _user.FullName),
            new Claim(ClaimTypes.NameIdentifier, _user.Id),
            new Claim(ClaimTypes.Email, _user.Email!),
            new Claim(ClaimTypes.Role, roleName!)
        };

            return claims;
        }
        public async Task<bool> LogUser(AppUser user)
        {
            var _user = await GetUserByEmail(user.Email!);
            if (_user == null) { return false; }

            string roleName=await role.GetUserRole(user!.Email!);
            if(string.IsNullOrEmpty(roleName))return false;
            return await userManager.CheckPasswordAsync(_user, user.PasswordHash!);

        }

        public async Task<int> RemoveUserByEmail(string email)
        {
            var user = await context.Users.FirstOrDefaultAsync(e => e.Email == email);
            if (user == null)
                return 0;

            context.Users.Remove(user);
            return await context.SaveChangesAsync();

        }
        public async Task<AppUser> GetUserById(string id)
        {
            var user =await userManager.FindByIdAsync(id);
            return user!;
        }
    }
}
