using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ForumApp2.Models
{
    public class ApplicationUser : IdentityUser
    {
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public bool IsBanned { get; set; }
    }

    
}
