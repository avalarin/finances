using System.ComponentModel.DataAnnotations;

namespace Finances.Models {
    public class Product {

        public int Id { get; set; }

        [Required]
        public Book Book { get; set; }

        [Required]
        [MaxLength(300)]
        public string Title { get; set; }

        [Required]
        public Unit Unit { get; set; }

    }
}