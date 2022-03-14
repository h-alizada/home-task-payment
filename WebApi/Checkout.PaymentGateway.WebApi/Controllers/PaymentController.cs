using Checkout.PaymentGateway.DomainServices.Core;
using Checkout.PaymentGateway.WebApi.Mappers;
using Checkout.PaymentGateway.WebApi.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Checkout.PaymentGateway.WebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PaymentController : ControllerBase
	{
		private readonly IPaymentService _service;
		public PaymentController(IPaymentService service)
		{
			_service = service;
		}

		[HttpPost]
		public async Task<ActionResult> ProcessPayment([FromBody] ProcessPaymentRequestDto request, CancellationToken cancellationToken)
		{

			var response = await _service.ProcessPayment(request.ToDomain());

			return Created("", response.ToProcessPaymentResponse());
		}

		[HttpGet("{id}")]
		public async Task<ActionResult> GetPaymentById([FromRoute]Guid id)
		{
			var response = await _service.GetPayment(id);

			return Ok(response.ToGetPaymentResponse());
		}
	}
}
