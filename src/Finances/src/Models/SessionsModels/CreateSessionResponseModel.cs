using System;

namespace Finances.Models.SessionsModels {
    public class CreateSessionResponseModel : ResponseModel<CreateSessionStatus> {

        public CreateSessionResponseModel(CreateSessionStatus status) : base(status) {
        }

        public CreateSessionResponseModel(Session session) : base(CreateSessionStatus.Success) {
            if (session == null) throw new ArgumentNullException(nameof(session));
            SessionKey = session.Id.ToString("N");
            ExpiresAt = session.ExpiresAt;
        }

        public string SessionKey { get; set; }

        public DateTime ExpiresAt { get; set; }

    }
}