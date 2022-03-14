using Checkout.PaymentGateway.Domain.Entities;
using Checkout.PaymentGateway.Domain.Exceptions;
using Checkout.PaymentGateway.Infrastructure.Services.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;

namespace Checkout.PaymentGateway.DomainServices.Tests
{
	[TestClass]
	public class PaymentServiceTest
	{
		private Mock<IPaymentInfrastructureService> _mockInfrastructure;

		[TestInitialize]
		public void Init()
		{
			_mockInfrastructure = new Mock<IPaymentInfrastructureService>();
		}


		[TestMethod]
		public async Task ProcessPayment_ProvidedCardIsOkayAndBankTransactionSucceed_SavePaymentAndReturn()
		{
			var validCardNumber = "0000-0000-0000-0001";
			var validCcv = "957";
			var bankTransactionId = Guid.NewGuid(); 
			var payment = new Payment
			{
				Card = new Card { CardNumber = validCardNumber, Cvv = validCcv, ExpiryMonth = 5, ExpiryYear = DateTime.Now.AddYears(10).Year }
			};

			_mockInfrastructure.Setup(x => x.ProcessPayment(It.IsAny<Card>(), It.IsAny<decimal>(), It.IsAny<CurrencyType>()))
				.ReturnsAsync(new BankTransaction
				{
					Id = bankTransactionId,
					PaymentStatus = PaymentStatus.Succeed
				});

			var service = new PaymentService(_mockInfrastructure.Object);

			await service.ProcessPayment(payment);

			_mockInfrastructure.Verify(x => x.SavePayment(payment), Times.Once);

			Assert.AreEqual(PaymentStatus.Succeed, payment.PaymentStatus);
			Assert.AreEqual(bankTransactionId, payment.Id);

		}

		[TestMethod]
		[ExpectedException(typeof(CardNumberIsNotValidException))]
		public async Task ProcessPayment_CardNumberIsNotValid_Throws()
		{
			var invalidCardNumber = "some-invailid-card";
			var service = new PaymentService(_mockInfrastructure.Object);

			await service.ProcessPayment(new Payment
			{
				Card = new Card { CardNumber = invalidCardNumber }
			}) ;
		}


		[TestMethod]
		[ExpectedException(typeof(CardCvvIsNotValidException))]
		public async Task ProcessPayment_CardCCVIsNotValid_Throws()
		{
			var validCardNumber = "0000-0000-0000-0001";
			var invlidCcv = "95789";
			var service = new PaymentService(_mockInfrastructure.Object);

			await service.ProcessPayment(new Payment
			{
				Card = new Card { CardNumber = validCardNumber, Cvv = invlidCcv }
			});
		}


		[TestMethod]
		[ExpectedException(typeof(CardExpiredException))]
		public async Task ProcessPayment_CardIsExpired_Throws()
		{
			var validCardNumber = "0000-0000-0000-0001";
			var validCcv = "957";
			var service = new PaymentService(_mockInfrastructure.Object);

			await service.ProcessPayment(new Payment
			{
				Card = new Card { CardNumber = validCardNumber, Cvv = validCcv, ExpiryMonth = 5, ExpiryYear = DateTime.Now.AddYears(-10).Year }
			});
		}


		[TestMethod]
		[ExpectedException(typeof(BankTransactionFailedException))]
		public async Task ProcessPayment_ProvidedCardIsOkayButBankTransactionFails_Throws()
		{
			var validCardNumber = "0000-0000-0000-0001";
			var validCcv = "957";

			_mockInfrastructure.Setup(x => x.ProcessPayment(It.IsAny<Card>(), It.IsAny<decimal>(), It.IsAny<CurrencyType>()))
				.ReturnsAsync(new BankTransaction
				{
					Id = Guid.NewGuid(),
					PaymentStatus = PaymentStatus.Failed
				});

			var service = new PaymentService(_mockInfrastructure.Object);

			await service.ProcessPayment(new Payment
			{
				Card = new Card { CardNumber = validCardNumber, Cvv = validCcv, ExpiryMonth = 5, ExpiryYear = DateTime.Now.AddYears(10).Year }
			});
		}

		[TestMethod]
		public async Task GetPayment_PaymentFound_ReturnPayment()
		{
			var payment = new Payment
			{
				UserId = Guid.NewGuid(),
				MerchantId = Guid.NewGuid(),
				Amount = 100,
				Currency = CurrencyType.EUR,
				Id = Guid.NewGuid(),
			};
			_mockInfrastructure.Setup(x => x.GetPayment(It.IsAny<Guid>()))
				.ReturnsAsync(payment);

			var service = new PaymentService(_mockInfrastructure.Object);

			var result = await service.GetPayment(It.IsAny<Guid>());

			Assert.AreEqual(result.Id, payment.Id);
			Assert.AreEqual(result.UserId, payment.UserId);
			Assert.AreEqual(result.MerchantId, payment.MerchantId);
			Assert.AreEqual(result.Amount, payment.Amount);
			Assert.AreEqual(result.Currency, payment.Currency);
		}

		[TestMethod]
		[ExpectedException(typeof(PaymentNotFoundException))]
		public async Task GetPayment_PaymentNotFound_Throws()
		{
			_mockInfrastructure.Setup(x => x.GetPayment(It.IsAny<Guid>()))
				.ReturnsAsync((Payment)null);

			var service = new PaymentService(_mockInfrastructure.Object);

			await service.GetPayment(It.IsAny<Guid>());
		}
	}
}
