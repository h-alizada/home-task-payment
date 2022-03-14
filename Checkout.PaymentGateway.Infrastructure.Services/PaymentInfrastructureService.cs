using Checkout.PaymentGateway.Domain.Entities;
using Checkout.PaymentGateway.Infrastructure.Core.ApiClients;
using Checkout.PaymentGateway.Infrastructure.Core.Encryptors;
using Checkout.PaymentGateway.Infrastructure.Core.Repositories;
using Checkout.PaymentGateway.Infrastructure.Services.Core;
using System;
using System.Threading.Tasks;

namespace Checkout.PaymentGateway.Infrastructure.Services
{
	public class PaymentInfrastructureService : IPaymentInfrastructureService
	{
		private readonly IPaymentRepository _repository;
		private readonly IBankClient _bankClient;
		private readonly ICryptographyService _cryptographyService;
		public PaymentInfrastructureService(IPaymentRepository repository, IBankClient bankClient, ICryptographyService cryptographyService)
		{
			_repository = repository;
			_bankClient = bankClient;
			_cryptographyService = cryptographyService;
		}

		public string DecryptCardNumber(string cardNumber)
		{
			return _cryptographyService.Decrypt(cardNumber);
		}

		public string EncryptCardNumber(string cardNumber)
		{
			return _cryptographyService.Encrypt(cardNumber);
		}

		public Task<Payment> GetPayment(Guid paymentId)
		{
			return _repository.GetPaymentById(paymentId);
		}



		public Task<BankTransaction> ProcessPayment(Card card, decimal amount, CurrencyType currency)
		{
			return _bankClient.ProcessPayment(card, amount, currency);
		}

		public Task SavePayment(Payment payment)
		{
			return _repository.SavePayment(payment);
		}
	}
}
