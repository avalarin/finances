using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Finances.Models {
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