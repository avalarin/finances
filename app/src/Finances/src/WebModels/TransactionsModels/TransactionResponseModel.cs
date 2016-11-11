using System;
using Finances.Models;

namespace Finances.WebModels.TransactionsModels {
    public class TransactionResponseModel<TStatus> : ResponseModel<TStatus> {

        public TransactionResponseModel(TStatus status) : base(status) {
        }

        public TransactionResponseModel(TStatus status, Transaction txn) : base(status) {
            if (txn == null) throw new ArgumentNullException(nameof(txn));
            Id = txn.Id;
        }

        public int Id { get; set; }

    }
}