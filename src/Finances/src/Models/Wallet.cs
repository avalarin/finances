using System.ComponentModel.DataAnnotations;

namespace Finances.Models {
    public class Wallet {
        
        [Key]
        public int Id { get; set; }

        [Required]
        public Book Book { get; set; }

    }
}