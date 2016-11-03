using System.ComponentModel.DataAnnotations;

namespace Finances.Models {
    public class Operation {
        
        [Key]
        public int Id { get; set; }

        public int TransactionId { get; set; }

        [Required]
        public Transaction Transaction { get; set; }

        public int WalletId { get; set; }

        public int Currency { get; set; }

        public decimal Amount { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }

        public decimal? Price { get; set; }

        public decimal? Count { get; set; }

    }
}