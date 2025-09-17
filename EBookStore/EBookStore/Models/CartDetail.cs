using System.ComponentModel.DataAnnotations;

namespace EBookStore.Models
{
    public class CartDetail
    {
        public int Id { get; set; }
        [Required]
        public int ShoopingCartId { get; set; }
        [Required]
        public int BookId { get; set; }
        [Required]
        public int Quantity { get; set; }
        public Book Book { get; set; }

        public ShoppingCart ShoppingCart { get; set; }

    }
}
