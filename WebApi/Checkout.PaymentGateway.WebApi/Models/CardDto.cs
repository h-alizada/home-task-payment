using System.ComponentModel.DataAnnotations;

namespace Checkout.PaymentGateway.WebApi.Models
{
	public class CardDto
	{
		[Required]
		public string CardNumber { get; set; }
		[Required]
		public int ExpiryMonth { get; set; }
		[Required]
		public int ExpiryYear { get; set; }
		[Required]
		public string Cvv { get; set; }
		[Required]
		public string HolderName { get; set; }
		[Required]
		public string CardType { get; set; }
	}
}
