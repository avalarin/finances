using System;

namespace Finances.Models.Responses {
    public class SessionResponseModel {

        public string SessionKey { get; set; }

        public DateTime ExpiresAt { get; set; }

        public string UserName { get; set; }

        public SessionResponseModel(Session session) {
            SessionKey = session.Id.ToString("N");
            ExpiresAt = session.ExpiresAt;
            UserName = session.User.UserName;
        }

    }
}