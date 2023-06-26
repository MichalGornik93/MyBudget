using System.ComponentModel.DataAnnotations;

namespace MyBudget.user
{
    public class Role
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
