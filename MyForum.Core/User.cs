using System;
using System.ComponentModel.DataAnnotations;

namespace MyForum.Core
{
    public class User
    {
        public int Id { get; set; }

        [Required, StringLength(40)]
        public string Nickname { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int PrestigePoints { get; set; }
    }
}
