using System.ComponentModel.DataAnnotations;

namespace Finances.Models {
    public class TransactionTag {

        public int TransactionId { get; set; }

        [Required]
        public Transaction Transaction { get; set; }

        public int TagId { get; set; }

        [Required]
        public Tag Tag { get; set; }

    }
}