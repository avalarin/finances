using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Finances.Models {
    public class Transaction {
        
        [Key]
        public int Id { get; set; }

        public int BookId { get; set; }

        [Required]
        public Book Book { get; set; }

        [Required]
        public ApplicationUser CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public ICollection<Operation> Operations { get; set; }

        public ICollection<TransactionTag> Tags { get; set; }

    }
}