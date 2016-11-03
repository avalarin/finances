using System.ComponentModel.DataAnnotations;

namespace Finances.Models {
    public class Product {

        public int Id { get; set; }

        public int BookId { get; set; }

        [Required]
        public Book Book { get; set; }

        [Required]
        [MaxLength(300)]
        public string Name { get; set; }

        [Required]
        public Unit Unit { get; set; }

    }
}