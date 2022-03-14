using Checkout.PaymentGateway.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Checkout.PaymentGateway.Infrastructure.Core.Repositories
{
	public interface IPaymentRepository
	{
		Task SavePayment(Payment payment);
		Task<Payment> GetPaymentById(Guid paymentId);
	}
}
