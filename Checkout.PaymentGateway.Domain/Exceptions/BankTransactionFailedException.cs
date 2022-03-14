using System;

namespace Checkout.PaymentGateway.Domain.Exceptions
{
    [Serializable]
	public class BankTransactionFailedException : Exception
	{
		public Guid BankTransactionId { get; set; }
		public BankTransactionFailedException()
        {
        }

        public BankTransactionFailedException(string message)
            : base(message)
        {
        }

        public BankTransactionFailedException(string message, Guid bankTransactionId)
            : base(message)
        {
            BankTransactionId = bankTransactionId;
        }
        public BankTransactionFailedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
