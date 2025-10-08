using TestCase.Entities;
using TestCase.Repository;

namespace TestCase.Services
{
    public class ProductService
    {
        private readonly IRepository<Product> _productRepository;

        public ProductService(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _productRepository.GetAll();
        }

        public Product? GetProductById(int id)
        {
            return _productRepository.GetById(id);
        }

        public void CreateProduct(Product product)
        {
            product.CreatedUser = "system";
            product.UpdateUser = "system";
            _productRepository.Add(product);
        }

        public void UpdateProduct(Product product)
        {
            product.UpdateUser = "system";
            _productRepository.Update(product);
        }

        public void DeleteProduct(int id)
        {
            var product = _productRepository.GetById(id);
            if (product != null)
            {
                _productRepository.Remove(product);
            }
        }
    }
}