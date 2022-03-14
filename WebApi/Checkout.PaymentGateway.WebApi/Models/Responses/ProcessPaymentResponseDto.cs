using System;

namespace Checkout.PaymentGateway.WebApi.Models.Responses
{
	public class ProcessPaymentResponseDto
	{
		public Guid Id { get; set; }
		public string Status { get; set; }
		public string ErrorMessage { get; set; }
	}
}
