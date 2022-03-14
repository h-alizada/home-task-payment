using Checkout.PaymentGateway.Domain.Exceptions;
using Checkout.PaymentGateway.WebApi.Models.Errors;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;

namespace Checkout.PaymentGateway.WebApi.Middleware
{
	public class ExceptionHandlerMiddleware
	{
		private readonly RequestDelegate _next;

		public ExceptionHandlerMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task Invoke(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (Exception ex)
			{
				await ConvertException(context, ex);
			}
		}

		private Task ConvertException(HttpContext context, Exception exception)
		{
			HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError;

			context.Response.ContentType = "application/json";

			var result = string.Empty;

			switch (exception)
			{
				case CardCvvIsNotValidException cardCvvIsNotValidException:
					httpStatusCode = HttpStatusCode.BadRequest;
					result = JsonConvert.SerializeObject(new ErrorResponseDto
					{
						ErrorMessage = cardCvvIsNotValidException.Message,
						Code = "400.100"
					});
					break;
				case CardExpiredException badRequestException:
					httpStatusCode = HttpStatusCode.BadRequest;
					result = JsonConvert.SerializeObject(new ErrorResponseDto
					{
						ErrorMessage = badRequestException.Message,
						Code = "400.101"
					});
					break;
				case CardNumberIsNotValidException cardNumberIsNotValidException:
					httpStatusCode = HttpStatusCode.BadRequest;
					result = JsonConvert.SerializeObject(new ErrorResponseDto
					{
						ErrorMessage = cardNumberIsNotValidException.Message,
						Code = "400.102"
					});
					break;
				case PaymentNotFoundException paymentNotFoundException:
					httpStatusCode = HttpStatusCode.NotFound;
					result = JsonConvert.SerializeObject(new ErrorResponseDto
					{
						ErrorMessage = paymentNotFoundException.Message,
						Code = "404.100"
					});
					break;
				case BankTransactionFailedException bankTransactionFailedException:
					httpStatusCode = HttpStatusCode.BadRequest;
					result = JsonConvert.SerializeObject(new BankTransactionFailedErrorDto
					{
						ErrorMessage = bankTransactionFailedException.Message,
						Code = "400.103",
						PaymentId = bankTransactionFailedException.BankTransactionId
					});
					break;
				case Exception ex:
					httpStatusCode = HttpStatusCode.InternalServerError;
					result = JsonConvert.SerializeObject(new ErrorResponseDto
					{
						ErrorMessage = ex.Message,

					});
					break;
			}

			context.Response.StatusCode = (int)httpStatusCode;

			if (result == string.Empty)
			{
				result = JsonConvert.SerializeObject(new { error = exception.Message });
			}
			Log.Error(JsonConvert.SerializeObject(
				new
				{
					error = exception.Message,
					stackTrace = exception.StackTrace,
					dateTime = DateTime.UtcNow,
					processName = Process.GetCurrentProcess().ProcessName,
					correlationId = Trace.CorrelationManager.ActivityId
				}));

			return context.Response.WriteAsync(result);
		}
	}
}
