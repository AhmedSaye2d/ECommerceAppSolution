using Microsoft.AspNetCore.Identity;

namespace ECommerceApp.Domain.Entities.Identity
{
    /// <summary>
    /// Represents an authenticated application user extending ASP.NET Core IdentityUser.
    /// </summary>
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
    }
}
