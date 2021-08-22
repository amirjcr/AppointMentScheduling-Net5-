using System;
using Microsoft.AspNetCore.Identity;



namespace Src.Models.CustomizeIdentity
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}