using System;

namespace Checkout.PaymentGateway.Domain.Entities
{
	public class BankTransaction
	{
		public Guid Id { get; set; }
		public PaymentStatus PaymentStatus { get; set; }
		public string ErrorMessage { get; set; }
	}
}
