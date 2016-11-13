namespace Finances.Exceptions {
    public class ApplicationError {

        public static readonly ApplicationError PermissionDenied = new ApplicationError("PermissionDenied", "Permission denied");
        public static readonly ApplicationError UserNotFound = new ApplicationError("UserNotFound", "User not found");
        public static readonly ApplicationError BookNotFound = new ApplicationError("BookNotFound", "Book not found");
        public static readonly ApplicationError AlreadyExists = new ApplicationError("AlreadyExists", "Object already exists");
        public static readonly ApplicationError BookUserNotFound = new ApplicationError("BookUserNotFound", "Book user not found");
        public static readonly ApplicationError Unauthenticated = new ApplicationError("Unauthenticated", "Unauthenticated");
        public static readonly ApplicationError Failed = new ApplicationError("Failed", "Failed");
        public static readonly ApplicationError InvalidNameOrPassword = new ApplicationError("InvalidNameOrPassword", "InvalidNameOrPassword");

        private ApplicationError(string name, string message) {
            Name = name;
            Message = message;
        }

        public string Name { get; }

        public string Message { get; }

        public ApplicationError CreateWithCustonMessage(string customMessage) {
            return new ApplicationError(Name, customMessage);
        }

        public override bool Equals(object obj) {
            if (obj == null || GetType() != obj.GetType()) {
                return false;
            }
            
            var anotherError = (ApplicationError)obj;

            return Name.Equals(anotherError.Name);
        }
        
        public override int GetHashCode() {
            return Name.GetHashCode();
        }

    }
}