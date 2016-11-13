namespace Finances.Models.Responses {
    public class UserResponseModel {
        public string Name { get; set; }

        public string Email { get; set; }

        public UserResponseModel(ApplicationUser user) {
            Name = user.UserName;
            Email = user.Email;
        }

    }
}