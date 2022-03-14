using System;

namespace Checkout.PaymentGateway.Domain.Exceptions
{
    [Serializable]
    public class CardCvvIsNotValidException : Exception
	{
        public CardCvvIsNotValidException()
        {
        }

        public CardCvvIsNotValidException(string message)
            : base(message)
        {
        }

        public CardCvvIsNotValidException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
