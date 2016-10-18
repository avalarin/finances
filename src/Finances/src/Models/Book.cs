using System.ComponentModel.DataAnnotations;

namespace Finances.Models {
    public class Book {

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        

    }
}
