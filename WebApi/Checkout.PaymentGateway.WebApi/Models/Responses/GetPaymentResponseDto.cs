using System;

namespace Checkout.PaymentGateway.WebApi.Models.Responses
{
	public class GetPaymentResponseDto
	{
		public Guid Id { get; set; }
		public Guid MerchantId { get; set; }
		public Guid UserId { get; set; }
		public decimal Amount { get; set; }
		public string Currency { get; set; }
		public string Status { get; set; }
		public CardDto Card { get; set; }
	}
}
