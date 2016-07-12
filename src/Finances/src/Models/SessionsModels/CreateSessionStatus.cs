namespace Finances.Models.SessionsModels {
    public enum CreateSessionStatus {
        Success,
        InvalidNameOrPassword,
        PermissionDenied,
        Failed
    }
}