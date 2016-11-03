using System;

namespace Finances.Services.ServiceOperations {
    public class OperationResultException : Exception {

        public object OperationResult { get; }

        public OperationResultException(object operationResult) {
            OperationResult = operationResult;
        }

        public OperationResultException(string message, object operationResult) : base(message) {
            OperationResult = operationResult;
        }

        public OperationResultException(string message, object operationResult, Exception inner) : base(message, inner) {
            OperationResult = operationResult;
        }
    }
}