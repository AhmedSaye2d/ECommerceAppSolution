using ECommerceApp.Domain.Entities.Identity;
using ECommerceApp.Domain.Interface.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Repository.Authentication
{
    public class RoleManagement(UserManager<AppUser> userManager) : IRoleManagement
    {
        public async Task<bool> AddUserToRole(AppUser user, string rolename)
          =>  (await userManager.AddToRoleAsync(user, rolename)).Succeeded;
        
        public async Task<string?> GetUserRole(string userEmail)
        {
            var user=await userManager.FindByEmailAsync(userEmail); 

            return (await userManager.GetRolesAsync(user!)).FirstOrDefault(); 
        }
       
    }
}
