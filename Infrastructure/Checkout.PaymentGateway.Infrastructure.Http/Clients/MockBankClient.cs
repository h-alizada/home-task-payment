using Checkout.PaymentGateway.Domain.Entities;
using Checkout.PaymentGateway.Infrastructure.Core.ApiClients;
using System;
using System.Threading.Tasks;

namespace Checkout.PaymentGateway.Infrastructure.Http.Clients
{
	public class MockBankClient : IBankClient
	{
		public Task<BankTransaction> ProcessPayment(Card cart, decimal amount, CurrencyType currency)
		{
			var transaction = new BankTransaction
			{
				Id = Guid.NewGuid(),
				PaymentStatus = PaymentStatus.Succeed
			};

			if (amount > 2500)
			{
				transaction.PaymentStatus = PaymentStatus.Failed;
				transaction.ErrorMessage = "Too much amount";
			}

			return Task.FromResult( transaction);
		}
	}
}
