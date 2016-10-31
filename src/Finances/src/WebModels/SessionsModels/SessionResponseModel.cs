using System;
using Finances.Models;

namespace Finances.WebModels.SessionsModels {
    public class SessionResponseModel : ResponseModel<CreateSessionStatus> {

        public SessionResponseModel(CreateSessionStatus status) : base(status) {
        }

        public SessionResponseModel(Session session) : base(CreateSessionStatus.Success) {
            if (session == null) throw new ArgumentNullException(nameof(session));
            SessionKey = session.Id.ToString("N");
            ExpiresAt = session.ExpiresAt;
            UserName = session.User.UserName;
        }

        public string SessionKey { get; set; }

        public DateTime ExpiresAt { get; set; }

        public String UserName { get; set; }

    }
}