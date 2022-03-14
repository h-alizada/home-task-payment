using Checkout.PaymentGateway.Domain.Entities;
using Checkout.PaymentGateway.Infrastructure.Sql.Entities;
using System;

namespace Checkout.PaymentGateway.Infrastructure.Sql.Mappers
{
	public static class PaymentMapper
	{
		public static Payment ToDomain(this PaymentEntity entity)
		{
			Enum.TryParse(entity.CardType, true, out CardType cardType);
			Enum.TryParse(entity.Currency, true, out CurrencyType currency);
			Enum.TryParse(entity.Status, true, out PaymentStatus paymentStatus);
			return new Payment
			{
				Id = entity.Id,
				PaymentStatus=paymentStatus,
				ErrorMessage = entity.ErrorMessage,
				UserId = entity.UserId,
				MerchantId = entity.MerchantId,
				CreatedDate = entity.CreatedDate,
			    Currency = currency,
				Amount = entity.Amount,
				Card = new Card
				{
					CardNumber = entity.CardNumber,
					CardType = cardType,
					Cvv = entity.CardCvv,
					ExpiryMonth = entity.CardExpiryMonth,
					ExpiryYear = entity.CardExpiryYear,
					HolderName = entity.CardHolderName,
					IsCardSaved = entity.IsCardSaved
				}
			};
		}

		public static PaymentEntity ToEntity(this Payment payment)
		{
			return new PaymentEntity
			{
				Id = payment.Id,
				UserId = payment.UserId,
				MerchantId = payment.MerchantId,
				Amount = payment.Amount,
				CardCvv = payment.Card.Cvv,
				CardExpiryMonth = payment.Card.ExpiryMonth,
				CardExpiryYear = payment.Card.ExpiryYear,
				CardHolderName = payment.Card.HolderName,
				CardNumber = payment.Card.CardNumber,
				CardType = payment.Card.CardType.ToString(),
				CreatedDate = payment.CreatedDate,
				Currency = payment.Currency.ToString(),
				ErrorMessage = payment.ErrorMessage,
				Status = payment.PaymentStatus.ToString(),
				IsCardSaved = payment.Card.IsCardSaved
			};
		}

	}
}
