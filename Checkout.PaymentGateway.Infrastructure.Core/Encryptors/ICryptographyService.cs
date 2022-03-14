namespace Checkout.PaymentGateway.Infrastructure.Core.Encryptors
{
	public interface ICryptographyService
	{
		string Encrypt(string plainText);
		string Decrypt(string cipherText);
	}
}
