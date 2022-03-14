using System;

namespace Checkout.PaymentGateway.Domain.Exceptions
{
    [Serializable]
	public class CardNumberIsNotValidException : Exception
	{
        public CardNumberIsNotValidException()
        {
        }

        public CardNumberIsNotValidException(string message)
            : base(message)
        {
        }

        public CardNumberIsNotValidException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
