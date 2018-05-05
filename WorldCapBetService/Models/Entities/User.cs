using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace WorldCapBetService.Models.Entities
{
    public class User : IdentityUser, IAuthorizationRequirement
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }

        public ICollection<Pronostic> Pronostics { get; set; }
    }
}
