using System;

namespace Checkout.PaymentGateway.WebApi.Models.Requests
{
	public class ProcessPaymentRequestDto
	{
		public Guid MerchantId { get; set; }
		public Guid UserId { get; set; }
		public decimal Amount { get; set; }
		public string Currency { get; set; }
		public CardDto Card { get; set; }
		public bool IsCardSaved { get; set; }
	}
}
