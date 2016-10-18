using System.ComponentModel.DataAnnotations;

namespace Finances.Models {
    public class Operation {
        
        [Key]
        public int Id { get; set; }

        [Required]
        public Transaction Transaction { get; set; }

        [Required]
        public Wallet Wallet { get; set; }

        [Required]
        public int Currency { get; set; }

        public decimal Amount { get; set; }
        
        public int? OriginalCurrency { get; set; }

        public decimal? OriginalAmount { get; set; }

        public decimal? ExchangeRate { get; set; }

        public ProductOperation ProductOperation { get; set; }

    }
}