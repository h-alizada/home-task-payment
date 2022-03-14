namespace Checkout.PaymentGateway.WebApi.Models
{
	public class CardDto
	{
		public string CardNumber { get; set; }
		public int ExpiryMonth { get; set; }
		public int ExpiryYear { get; set; }
		public string Cvv { get; set; }
		public string HolderName { get; set; }
		public string CardType { get; set; }
	}
}
