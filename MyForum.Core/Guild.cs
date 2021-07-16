using System.ComponentModel.DataAnnotations;

namespace MyForum.Core
{
    public class Guild
    {
        public int Id { get; set; }

        [Required, StringLength(80)]
        public string Name { get; set; }
    }  
}
