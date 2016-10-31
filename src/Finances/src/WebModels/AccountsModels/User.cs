using Finances.Models;

namespace Finances.WebModels.AccountsModels {
    public class User {
        public string Name { get; set; }

        public string Email { get; set; }

        public static User FromApplicationUser(ApplicationUser user) {
            return new User() {
                Name = user.UserName,
                Email = user.Email
            };
        }

    }
}