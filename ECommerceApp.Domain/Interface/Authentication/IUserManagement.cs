using ECommerceApp.Domain.Entities.Identity;
using System.Security.Claims;

namespace ECommerceApp.Domain.Interface.Authentication
{
    public interface IUserManagement
    {
        Task<bool> CreateUser(AppUser user);
        Task<bool> LogUser(AppUser user);
        Task<AppUser> GetUserByEmail(string email);
        Task<AppUser> GetUserById(string id);   
        Task<IEnumerable<AppUser>?> GetAllUsers();
        Task<int> RemoveUserByEmail(string email);
        Task<List<Claim>> GetUserClaim(string email);

    }
}
