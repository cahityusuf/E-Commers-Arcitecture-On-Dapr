using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Order.Infrastructure.DbContexts
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
        {
        }

        public DbSet<Domain.AggregatesModel.OrderAggregate.Order> Order => Set<Domain.AggregatesModel.OrderAggregate.Order>();
        public DbSet<Domain.AggregatesModel.OrderItemAggregate.OrderItem> OrderItem => Set<Domain.AggregatesModel.OrderItemAggregate.OrderItem>();
        public DbSet<Domain.AggregatesModel.AddressAggregate.Address> Address => Set<Domain.AggregatesModel.AddressAggregate.Address>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
