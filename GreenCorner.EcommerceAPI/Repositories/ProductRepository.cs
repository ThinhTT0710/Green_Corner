using GreenCorner.EcommerceAPI.Data;
using GreenCorner.EcommerceAPI.Models;
using GreenCorner.EcommerceAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace GreenCorner.EcommerceAPI.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly GreenCornerEcommerceContext _context;
        public ProductRepository(GreenCornerEcommerceContext context)
        {
            _context = context;
        }
        public async Task<Product> AddProduct(Product item)
        {
            await _context.Products.AddAsync(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                throw new Exception($"Product with ID {id} not found.");
            }
            //product.IsDeleted = true;
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> GetAllProduct()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetByProductId(int id)
        {
            return await _context.Products
                .FirstOrDefaultAsync(p => p.ProductId == id)
                ?? throw new Exception($"Product with ID {id} not found.");
        }

        public async Task UpdateProduct(Product item)
        {
            var product = await _context.Products.FindAsync(item.ProductId);
            if (product == null)
            {
                throw new Exception($"Product with ID {item.ProductId} not found.");
            }
            _context.Entry(product).CurrentValues.SetValues(item);
            product.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Product>> GetNewestProducts()
        {
            return await _context.Products
                .Where(p => p.IsDeleted == false && p.Quantity > 0)
                .OrderByDescending(p => p.CreatedAt)                   
                .Take(10)                                               
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetOutOfStockProduct()
        {
            return await _context.Products
                .Where(p => p.Quantity <= 0 && p.IsDeleted == false)
                .OrderByDescending(p => p.CreatedAt)
                .Take(10)
                .ToListAsync();
        }
    }
}
