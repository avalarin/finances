using System.ComponentModel.DataAnnotations;

namespace Finances.Models.Requests {
    public class GetWalletsRequestModel {
        [Required]
        public int BookId { get; set; }
    }
}