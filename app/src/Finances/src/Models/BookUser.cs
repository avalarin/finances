using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Finances.Models {
    public class BookUser {

        [Key]
        public int Id { get; set; }

        public int BookId { get; set; }

        public Book Book { get; set; }
            
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        public BookUserRole Role { get; set; }

    }
}