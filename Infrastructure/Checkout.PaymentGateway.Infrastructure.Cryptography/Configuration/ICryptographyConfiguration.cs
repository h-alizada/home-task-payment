namespace Checkout.PaymentGateway.Infrastructure.Cryptography.Configuration
{
	public interface ICryptographyConfiguration
	{
		string Key { get; }
		string Vector { get; }
	}
}
