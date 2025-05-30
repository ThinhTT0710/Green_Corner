using Microsoft.AspNetCore.Identity;

namespace GreenCorner.AuthAPI.Models
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Avatar { get; set; }
    }
}
