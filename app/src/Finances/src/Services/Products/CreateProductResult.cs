using Finances.Models;

namespace Finances.Services.Products {
    public class CreateProductResult : OperationResult<CreateProductErrorCode, Product> {

        public Product Product { get; }

        protected override Product Payload => Product;

        public CreateProductResult(CreateProductErrorCode? errorCode) : base(errorCode) {
        }

        public CreateProductResult(Product product) {
            Product = product;
        }
    }
}