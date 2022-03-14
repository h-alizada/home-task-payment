using Checkout.PaymentGateway.Domain.Entities;
using Checkout.PaymentGateway.Infrastructure.Core.Repositories;
using Checkout.PaymentGateway.Infrastructure.Sql.Context;
using Checkout.PaymentGateway.Infrastructure.Sql.Mappers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Checkout.PaymentGateway.Infrastructure.Sql.Repositories
{
	public class PaymentRepository : IPaymentRepository
	{
		private readonly PaymentGatewayContext _context;
		public PaymentRepository(PaymentGatewayContext context)
		{
			_context = context;
		}
		public async Task<Payment> GetPaymentById(Guid paymentId)
		{
			var entity = await _context.Payments.FirstOrDefaultAsync(x => x.Id == paymentId);

			return entity?.ToDomain();
		}

		public Task SavePayment(Payment payment)
		{
			_context.Payments.AddAsync(payment.ToEntity());

			return _context.SaveChangesAsync();
		}
	}
}
