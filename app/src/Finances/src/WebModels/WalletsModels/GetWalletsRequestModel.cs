using System.ComponentModel.DataAnnotations;

namespace Finances.WebModels.WalletsModels {
    public class GetWalletsRequestModel {
        [Required]
        public int BookId { get; set; }
    }
}