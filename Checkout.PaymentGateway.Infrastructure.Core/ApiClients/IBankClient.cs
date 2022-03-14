using Checkout.PaymentGateway.Domain.Entities;
using System.Threading.Tasks;

namespace Checkout.PaymentGateway.Infrastructure.Core.ApiClients
{
	public interface IBankClient
	{
		Task<BankTransaction> ProcessPayment(Card cart, decimal amount, CurrencyType currency);
	}
}
