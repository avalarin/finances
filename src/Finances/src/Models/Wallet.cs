using System.ComponentModel.DataAnnotations;

namespace Finances.Models {
    public class Wallet {
        
        [Key]
        public int Id { get; set; }

        public int BookId { get; set; }

        [Required]
        public Book Book { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 1)]
        public string Name { get; set; }
    }
}