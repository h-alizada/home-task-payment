using Microsoft.Extensions.Configuration;

namespace Checkout.PaymentGateway.Infrastructure.Cryptography.Configuration
{
	public class CryptographyConfiguration : ICryptographyConfiguration
    {
        public CryptographyConfiguration(IConfiguration configuration)
        {
            configuration.GetSection(nameof(CryptographyConfiguration)).Bind(this);
        }
        public string Key { get; set; }
        public string Vector { get; set; }
    }
}
