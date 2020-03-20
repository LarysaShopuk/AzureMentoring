using System.Data.Entity;

namespace ProductAPI.DAL
{
    public class AdventureWorksContext : DbContext
    {
        public AdventureWorksContext(string connectionString)
            : base(connectionString)
        {
        }

        public DbSet<Product> Products { get; set; }
    }
}
