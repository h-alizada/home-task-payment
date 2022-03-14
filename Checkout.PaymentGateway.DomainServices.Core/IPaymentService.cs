using Checkout.PaymentGateway.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Checkout.PaymentGateway.DomainServices.Core
{
	public interface IPaymentService
	{
		Task<Payment> ProcessPayment(Payment payment);
		Task<Payment> GetPayment(Guid paymentId);
	}
}
