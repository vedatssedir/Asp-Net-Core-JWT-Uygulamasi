using System.Collections.Generic;

namespace WebApplication.Models
{
    public class AppUser
    {
        public string FullName { get; set; }
        public string Username { get; set; }
        public List<string> Roles { get; set; }
    }
}