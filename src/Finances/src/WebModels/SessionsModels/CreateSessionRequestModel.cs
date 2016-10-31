using System.ComponentModel.DataAnnotations;

namespace Finances.WebModels.SessionsModels {
    public class CreateSessionRequestModel {
         
        [Required]
        [MaxLength(100)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(100)]
        public string Password { get; set; }
        
    }
}