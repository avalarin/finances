using System.ComponentModel.DataAnnotations;

namespace Finances.Models {
    public class Unit {

        [Key]
        public int Id { get; set; }

        public Book Book { get; set; }

        [Required]
        [MaxLength(50)]
        public string Code { get; set; }

        [MaxLength(100)]
        public string Text { get; set; }

        public int Decimals { get; set; }

    }
}