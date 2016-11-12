using System;
using Finances.Exceptions;
using Microsoft.Extensions.Logging;

namespace Finances.Utils.Logging {
    
    public static class LoggerExtensions {
        
        public static void LogAppErrorAndThrow(this ILogger logger, ApplicationError error, Exception inner) {
            logger.LogError(null, inner, error.Message);
            throw new ApplicationException(error, inner);
        }

        public static void LogAppErrorAndThrow(this ILogger logger, ApplicationError error) {
            logger.LogError(error.Message);
            throw new ApplicationException(error);
        }

        public static void LogAppErrorAndThrow(this ILogger logger, string customMessage, ApplicationError error) {
            LogAppErrorAndThrow(logger, error.CreateWithCustonMessage(customMessage));
        }

        public static void LogAppErrorAndThrow(this ILogger logger, string customMessage,ApplicationError error, Exception inner) {
            LogAppErrorAndThrow(logger, error.CreateWithCustonMessage(customMessage), inner);
        }

    }

}