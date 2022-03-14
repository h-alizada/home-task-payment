using System;
using System.ComponentModel.DataAnnotations;

namespace Checkout.PaymentGateway.WebApi.Models.Requests
{
	public class ProcessPaymentRequestDto
	{
		[Required]
		public Guid MerchantId { get; set; }
		[Required]
		public Guid UserId { get; set; }
		[Required]
		public decimal Amount { get; set; }
		[Required]
		public string Currency { get; set; }
		[Required]
		public CardDto Card { get; set; }
		[Required]
		public bool IsCardSaved { get; set; }
	}
}
