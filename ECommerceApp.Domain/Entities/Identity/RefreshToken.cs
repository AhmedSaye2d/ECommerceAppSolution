using System;

namespace ECommerceApp.Domain.Entities.Identity
{
    /// <summary>
    /// Represents a persistent cryptographic refresh token for single-use token rotation.
    /// </summary>
    public class RefreshToken
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Token { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;
    }
}
