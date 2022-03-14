namespace Checkout.PaymentGateway.Domain.Entities
{
	public class Card
	{
		public string CardNumber { get; set; }
		public int ExpiryMonth { get; set; }
		public int ExpiryYear { get; set; }
		public string Cvv { get; set; }
		public string HolderName { get; set; }
		public CardType CardType { get; set; }
		public bool IsCardSaved { get; set; }
	}
}
