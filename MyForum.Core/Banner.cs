using System.ComponentModel.DataAnnotations;

namespace MyForum.Core
{
    public class Banner
    {
        public int Id { get; set; }

        [Required, StringLength(80)]
        public string Name { get; set; }
    }  
}
