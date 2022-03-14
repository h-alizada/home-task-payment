using System;

namespace Checkout.PaymentGateway.WebApi.Models.Errors
{
	public class BankTransactionFailedErrorDto : ErrorResponseDto
	{
		public Guid PaymentId { get; set; }
	}
}
