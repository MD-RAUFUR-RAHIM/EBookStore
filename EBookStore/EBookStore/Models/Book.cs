using System.ComponentModel.DataAnnotations;

namespace EBookStore.Models
{
    public class Book
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(40)]
        public string? BookName { get; set; }
        [Required]
        public double  Price { get; set; }
        public string? BookImage { get; set; }
        [Required]
        public int GenreId { get; set; }
        public Genre Genre { get; set; }
        public List<OrderDetails> OrderDetails { get; set; }
        public List<CartDetail> CartDetail { get; set; }

    }
}
