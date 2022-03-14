using Checkout.PaymentGateway.Domain.Entities;
using Checkout.PaymentGateway.WebApi.Models.Requests;
using Checkout.PaymentGateway.WebApi.Models.Responses;
using System;

namespace Checkout.PaymentGateway.WebApi.Mappers
{
	public static class PaymentMapper
	{
		public static Payment ToDomain(this ProcessPaymentRequestDto requestDto)
		{

			return new Payment
			{
				Amount = requestDto.Amount,
				MerchantId = requestDto.MerchantId,
				UserId = requestDto.UserId,
				Currency = (CurrencyType)Enum.Parse(typeof(CurrencyType), requestDto.Currency, true),
				Card = new Card
				{
					CardNumber = requestDto.Card.CardNumber,
					Cvv = requestDto.Card.Cvv,
					ExpiryMonth = requestDto.Card.ExpiryMonth,
					ExpiryYear = requestDto.Card.ExpiryYear,
					HolderName = requestDto.Card.HolderName,
					IsCardSaved = requestDto.IsCardSaved,
					CardType = (CardType)Enum.Parse(typeof(CardType), requestDto.Card.CardType, true),
				}
			};

		}

		public static ProcessPaymentResponseDto ToProcessPaymentResponse(this Payment payment)
		{
			return new ProcessPaymentResponseDto
			{
				Id = payment.Id,
				Status = payment.PaymentStatus.ToString(),
				ErrorMessage = payment.ErrorMessage
			};
		}

		public static GetPaymentResponseDto ToGetPaymentResponse(this Payment payment)
		{
			return new GetPaymentResponseDto
			{
				Amount = payment.Amount,
				Id = payment.Id,
				Currency = payment.Currency.ToString(),
				Status = payment.PaymentStatus.ToString(),
				UserId = payment.UserId,
				MerchantId = payment.MerchantId,
				Card = new Models.CardDto
				{
					CardNumber = payment.Card.CardNumber,
					CardType = payment.Card.CardType.ToString(),
					Cvv	= payment.Card.Cvv,
					ExpiryMonth = payment.Card.ExpiryMonth,
					ExpiryYear = payment.Card.ExpiryYear,
					HolderName = payment.Card.HolderName
				}
			};
		}
	}
}
