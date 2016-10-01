using System;

namespace Finances.Services {
    public abstract class OperationResult<TErrorCode, TPayload> : IOperationResult
        where TErrorCode : struct {

        public TErrorCode? ErrorCode { get; }

        protected abstract TPayload Payload { get; }

        public virtual bool Success => !ErrorCode.HasValue;

        protected OperationResult(TErrorCode? errorCode) {
            ErrorCode = errorCode;
        }

        protected OperationResult() {
            ErrorCode = null;
        }

        public TPayload EnsureSuccess() {
            if (!Success) {
                throw new InvalidOperationException(ErrorCode.ToString());
            }
        
            return Payload;
        }

    }

    public interface IOperationResult {
        
        bool Success { get; }

    }
}
