using System;

namespace Checkout.PaymentGateway.Domain.Entities
{
	public class Payment
	{
		public Guid Id { get; set; }
		public PaymentStatus PaymentStatus { get; set; }
		public string ErrorMessage { get; set; }
		public Guid MerchantId { get; set; }
		public Guid UserId { get; set; }
		public decimal Amount { get; set; }
		public CurrencyType Currency { get; set; }
		public DateTime CreatedDate { get; set; }
		public Card Card { get; set; }
	}
}
