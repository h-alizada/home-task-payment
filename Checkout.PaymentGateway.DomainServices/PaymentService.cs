using Checkout.PaymentGateway.Domain.Entities;
using Checkout.PaymentGateway.Domain.Exceptions;
using Checkout.PaymentGateway.DomainServices.Core;
using Checkout.PaymentGateway.Infrastructure.Services.Core;
using System;
using System.Threading.Tasks;

namespace Checkout.PaymentGateway.DomainServices
{
	public class PaymentService : IPaymentService
	{
		private readonly IPaymentInfrastructureService _infrastructureService;
		public PaymentService(IPaymentInfrastructureService infrastructureService)
		{
			_infrastructureService = infrastructureService;
		}

		public async Task<Payment> ProcessPayment(Payment payment)
		{

			ValidateCardNumber(payment.Card.CardNumber);
			ValidateCardCvv(payment.Card.Cvv);
			ValdiateCardExpiry(payment.Card.ExpiryYear, payment.Card.ExpiryMonth);

			var bankTransaction = await _infrastructureService.ProcessPayment(payment.Card, payment.Amount, payment.Currency);

			payment.Id = bankTransaction.Id;
			payment.PaymentStatus = bankTransaction.PaymentStatus;
			payment.ErrorMessage = bankTransaction.ErrorMessage;
			payment.CreatedDate = DateTime.UtcNow;

			var returnedCardNumber = $"********{payment.Card.CardNumber.Substring(payment.Card.CardNumber.Length - 4)}";
			payment.Card.CardNumber = _infrastructureService.EncryptCardNumber(payment.Card.CardNumber);

			await _infrastructureService.SavePayment(payment);

			if (bankTransaction.PaymentStatus != PaymentStatus.Succeed)
			{
				throw new BankTransactionFailedException(bankTransaction.ErrorMessage, bankTransaction.Id);
			}

			payment.Card.CardNumber = returnedCardNumber;

			return payment;
		}

		public async Task<Payment> GetPayment(Guid paymentId)
		{
			var payment = await _infrastructureService.GetPayment(paymentId);

			if (payment == null)
			{
				throw new PaymentNotFoundException($"Payment with the id '{paymentId}' not found");
			}

			var cardNumber = _infrastructureService.DecryptCardNumber(payment.Card.CardNumber);
			payment.Card.CardNumber = $"********{cardNumber.Substring(cardNumber.Length - 4)}";


			return payment;
		}

		private void ValidateCardNumber(string cardNumber)
		{
			cardNumber = cardNumber.Replace("-", string.Empty);
			//just check if card consists of 16 digit before making api call to Bank API
			//checking if it is valid master or visa or any other card should be the responsibility of Bank API
			if (cardNumber.Length != 16 || !long.TryParse(cardNumber, out long result))
			{
				throw new CardNumberIsNotValidException($"Given card number '{cardNumber}' is not valid");
			}
		}

		private void ValidateCardCvv(string cvv)
		{
			if (cvv.Length != 3 || !char.IsDigit(cvv[0]) || !char.IsDigit(cvv[1]) || !char.IsDigit(cvv[2]))
			{
				throw new CardCvvIsNotValidException($"Given card ccv number '{cvv}' is not valid");
			}
		}

		private void ValdiateCardExpiry(int expiryYear, int expiryMonth)
		{
			var expiryDate = new DateTime(expiryYear, expiryMonth, 1);
			var currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

			if (expiryDate < currentDate)
			{
				throw new CardExpiredException($"Given is expired at '{expiryDate}'");
			}
		}
	}
}
