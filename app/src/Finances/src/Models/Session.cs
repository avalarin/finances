using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Finances.Models {
    public class Session {

        [Key]
        public Guid Id { get; set; }

        [Required]
        public ApplicationUser User { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ExpiresAt { get; set; }

        public DateTime? ClosedAt { get; set; }

        [NotMapped]
        public bool IsActual => ExpiresAt > DateTime.Now && !ClosedAt.HasValue;
    }
}