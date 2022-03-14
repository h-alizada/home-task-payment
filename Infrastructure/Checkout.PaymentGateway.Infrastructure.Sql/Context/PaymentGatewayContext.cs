using Checkout.PaymentGateway.Infrastructure.Sql.Entities;
using Microsoft.EntityFrameworkCore;

namespace Checkout.PaymentGateway.Infrastructure.Sql.Context
{
	public  class PaymentGatewayContext : DbContext
	{
		
		public PaymentGatewayContext(DbContextOptions<PaymentGatewayContext> options)
		   : base(options)
		{
			Database.EnsureCreated();
		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.ApplyConfigurationsFromAssembly(typeof(PaymentGatewayContext).Assembly);
			modelBuilder.Entity<PaymentEntity>().Property(e => e.ErrorMessage).IsRequired(false);
		}

		public DbSet<PaymentEntity> Payments { get; set; }
	}
}
