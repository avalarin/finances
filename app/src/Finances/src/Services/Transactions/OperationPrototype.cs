namespace Finances.Services.Transactions {
    public class OperationPrototype {
        
        public int WalletId { get; set; }

        public int Currency { get; set; }

        public decimal Amount { get; set; }

        public int? ProductId { get; set; }

        public ProductPrototype Product { get; set; }

        public decimal? Price { get; set; }

        public decimal? Count { get; set; }

    }
}