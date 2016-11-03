namespace Finances.WebModels.SessionsModels {
    public enum CreateSessionStatus {
        Success,
        InvalidNameOrPassword,
        PermissionDenied,
        Failed,
        Unauthenticated
    }
}