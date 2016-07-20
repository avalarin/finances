using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Finances.Models {
    public class Tag {

        [Key]
        public int Id { get; set; }

        [Required]
        public Book Book { get; set; }

        [Required]
        [MaxLength(100)]
        public string Text { get; set; }

        public ICollection<TransactionTag> Transactions { get; set; }

    }
}