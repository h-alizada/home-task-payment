using System;

namespace Checkout.PaymentGateway.Domain.Exceptions
{
    [Serializable]
	public class CardExpiredException : Exception
	{
        public CardExpiredException()
        {
        }

        public CardExpiredException(string message)
            : base(message)
        {
        }

        public CardExpiredException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
