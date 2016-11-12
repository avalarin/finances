using System;

namespace Finances.Models.Prototypes {
    public class TransactionPrototype {
        
        public int BookId { get; set; }

        public String UserName { get; set; }

        public DateTime DateTime { get; set; }
        
        public string[] Tags { get; set; }

        public OperationPrototype[] Operations { get; set; }

    }
}