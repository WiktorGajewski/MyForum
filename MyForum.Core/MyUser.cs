using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace MyForum.Core
{
    public class MyUser : IdentityUser
    {
        public DateTime RegistrationDate { get; set; }
        public int PrestigePoints { get; set; }
        public Rank Rank { get; set; }
    }
}
