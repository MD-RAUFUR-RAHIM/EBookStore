using System.ComponentModel.DataAnnotations;

namespace EBookStore.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }
       
        [Required]
        public string UserId { get; set; }
        public int MyProperty { get; set; }
        public bool IsDeleted { get; set; }
    }
}
