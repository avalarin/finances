using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Finances.Data;
using Microsoft.Extensions.Logging;

namespace Finances.Services.ServiceOperations {
    public abstract class OperationBase<TResult, TOptions>
        where TResult : class, IOperationResult {

        private readonly Dictionary<Type, Func<Exception, TResult>> _errorsMap = new Dictionary<Type, Func<Exception, TResult>>(); 

        protected ILogger Logger { get; }

        protected ApplicationDbContext DataBase { get; }

        protected OperationBase(ILogger logger, ApplicationDbContext database) {
            Logger = logger;
            DataBase = database;
        }

        public async Task<TResult> Execute(TOptions options) {
            try {
                return await ExecuteCore(options);
            }
            catch (OperationResultException operationResultException) {
                if (operationResultException.OperationResult == null) {
                    throw new InvalidOperationException("OperationResult cannot be null");
                }
                var result = operationResultException.OperationResult as TResult;
                if (result == null) {
                    throw new InvalidCastException($"Cannot cast {operationResultException.OperationResult.GetType()} to {typeof(TResult)}");
                }
                return result;
            }
            catch (Exception e) {
                var result = ConvertException(e);
                if (result == null) {
                    Logger.LogError($"Unexpected exception occured while {GetType()} was executing");
                    throw;
                }
                return result;
            }
        }

        protected abstract Task<TResult> ExecuteCore(TOptions options);

        protected void MapException<TException>(Func<TException, TResult> func)
            where TException : Exception {
            if (func == null) throw new ArgumentNullException(nameof(func));
            if (_errorsMap.ContainsKey(typeof (TException))) {
                throw new InvalidOperationException($"Exception {typeof(TException)} already mapped");
            }
            _errorsMap[typeof (TException)] = e => {
                var castedException = e as TException;
                if (castedException == null) {
                    throw new InvalidCastException($"Cannot cast {e.GetType()} to {typeof(TException)}");
                }
                return func(castedException);
            };
        }

        private TResult ConvertException(Exception e) {
            Func<Exception, TResult> func;
            if (!_errorsMap.TryGetValue(e.GetType(), out func)) {
                return null;
            }
            return func(e);
        }
    }
}