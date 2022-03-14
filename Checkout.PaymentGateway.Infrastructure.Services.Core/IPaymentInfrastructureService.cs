using Checkout.PaymentGateway.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Checkout.PaymentGateway.Infrastructure.Services.Core
{
	public interface IPaymentInfrastructureService
	{
		Task<BankTransaction> ProcessPayment(Card cart, decimal amount, CurrencyType currency);
		Task SavePayment(Payment payment);
		Task<Payment> GetPayment(Guid paymentId);
		string EncryptCardNumber(string cardNumber);
		string DecryptCardNumber(string cardNumber);
	}
}
