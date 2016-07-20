using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Finances.Models {
    public class Book {

        [Key]
        public int Id { get; set; }

    }

    public class Wallet {
        
        [Key]
        public int Id { get; set; }

        [Required]
        public Book Book { get; set; }

    }

    public class Currency {
        
        [Key]
        public int Id { get; set; }

        public Book Book { get; set; }

        [Required]
        [MaxLength(50)]
        public string Code { get; set; }

        [MaxLength(100)]
        public string Text { get; set; }

    }

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

    public class Transaction {
        
        [Key]
        public int Id { get; set; }

        [Required]
        public Book Book { get; set; }

        [Required]
        public ApplicationUser CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public ICollection<TransactionTag> Tags { get; set; }

    }

    public class TransactionTag {

        public int TransactionId { get; set; }

        [Required]
        public Transaction Transaction { get; set; }

        public int TagId { get; set; }

        [Required]
        public Tag Tag { get; set; }

    }

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

    public class Operation {
        
        [Key]
        public int Id { get; set; }

        [Required]
        public Transaction Transaction { get; set; }

        [Required]
        public Wallet Wallet { get; set; }

        [Required]
        public Currency Currency { get; set; }

        public decimal Amount { get; set; }
        
        public Currency OriginalCurrency { get; set; }

        public decimal? OriginalAmount { get; set; }

        public decimal? ExchangeRate { get; set; }

        public ProductOperation ProductOperation { get; set; }

    }

    public class ProductOperation {

        [Key]
        [ForeignKey("Operation")]
        public int Id { get; set; }

        [Required]
        public Operation Operation { get; set; }

        public int OperationId { get; set; }

        [Required]
        public Product Product { get; set; }

        public decimal Price { get; set; }

        public decimal Count { get; set; }

    }

}
