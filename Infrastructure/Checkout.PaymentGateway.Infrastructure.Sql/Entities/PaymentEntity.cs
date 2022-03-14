using System;

namespace Checkout.PaymentGateway.Infrastructure.Sql.Entities
{
	public class PaymentEntity
	{
		public Guid Id { get; set; }
		public string Status { get; set; }
		public string ErrorMessage { get; set; }
		public decimal Amount { get; set; }
		public string Currency { get; set; }
		public Guid MerchantId { get; set; }
		public Guid UserId { get; set; }
		public DateTime CreatedDate { get; set; }
		public string CardNumber { get; set; }
		public int CardExpiryMonth { get; set; }
		public int CardExpiryYear { get; set; }
		public string CardCvv { get; set; }
		public string CardHolderName { get; set; }
		public string CardType { get; set; }
		public bool IsCardSaved { get; set; }
	}
}
