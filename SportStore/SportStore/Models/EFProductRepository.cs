using System.Linq;
using System.Collections.Generic;


namespace SportStore.Models
{
    public class EFProductRepository :IProductRepository
    {
        private ApplicationDbContext context;
        public EFProductRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }
        public IEnumerable<Product> Products => context.Products;

        public void SaveProduct(Product product)
        {
            if (product.ProductID == 0)
            {
                context.Products.Add(product);
            }
            else
            {
                Product DbEntry = context.Products.FirstOrDefault(p => p.ProductID == product.ProductID);
                if (DbEntry != null)
                {
                    DbEntry.Name = product.Name;
                    DbEntry.Description = product.Description;
                    DbEntry.Price = product.Price;
                    DbEntry.Category = product.Category;
                }
            }
            context.SaveChanges();
        }

        public Product DeleteProduct(int productId)
        {
            Product DbEntry = context.Products.FirstOrDefault(p => p.ProductID == productId);
            if (DbEntry != null)
            {
                context.Products.Remove(DbEntry);
                context.SaveChanges();
            }
            return DbEntry;
        }
    }
}
