using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Finances.Data;
using Finances.Models;
using Finances.Services.Books;
using Finances.Services.Users;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace Finances.Services.Transactions {
    public interface ITransactionStore {

        Task CreateTransaction(TransactionPrototype transaction);

    }
}