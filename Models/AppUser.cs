using Microsoft.AspNetCore.Identity;

namespace IdentitiyExample.Models
{
    public class AppUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

    }
}
