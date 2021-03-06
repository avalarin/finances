﻿using System.ComponentModel.DataAnnotations;

namespace Finances.Models.Requests {
    public class CreateWalletRequestModel {
        [Required]
        [StringLength(200, MinimumLength = 1)]
        public string WalletName { get; set; }

        [Required]
        public int BookId { get; set; }
    }
}