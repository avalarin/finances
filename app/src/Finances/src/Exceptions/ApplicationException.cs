using System;

namespace Finances.Exceptions {

    public class ApplicationException : System.Exception {
        public ApplicationException(ApplicationError error) : base(error.Message) {
            if (error == null) throw new ArgumentException(nameof(error));
            Error = error;
        }
        public ApplicationException(ApplicationError error, System.Exception inner) : base(error.Message, inner) {
            if (error == null) throw new ArgumentException(nameof(error));
            Error = error;
        }

        public ApplicationError Error { get; }
    }
}