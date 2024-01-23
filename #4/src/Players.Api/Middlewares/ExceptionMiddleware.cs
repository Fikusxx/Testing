using Players.Application.Common.Exceptions;
using Players.Domain.Common.Exceptions;
using Players.Domain.Common;
using Newtonsoft.Json;
using System.Net.Mime;
using System.Net;

namespace Players.Api.Middlewares;

public sealed class ExceptionMiddleware
{
	private readonly RequestDelegate next;

	public ExceptionMiddleware(RequestDelegate next)
	{
		this.next = next;
	}

	public async Task InvokeAsync(HttpContext context)
	{
		try
		{
			await next(context);
		}
		catch (Exception ex)
		{

			await HandleExceptionAsync(context, ex);
		}
	}

	private Task HandleExceptionAsync(HttpContext context, Exception exception)
	{
		context.Response.ContentType = MediaTypeNames.Application.Json;
		HttpStatusCode statusCode = HttpStatusCode.InternalServerError;

		var errorDetails = new ExceptionDetails()
		{
			ErrorMessage = exception.Message,
			ErrorType = exception.GetType().Name,
			TraceId = context.TraceIdentifier
		};

		string result = JsonConvert.SerializeObject(errorDetails);

		switch (exception)
		{
			case NotFoundException _:
				statusCode = HttpStatusCode.NotFound;
				break;

			case BusinessRuleValidationException _:
			case DomainException _:
				statusCode = HttpStatusCode.BadRequest;
				break;

			default:
				break;
		}

		context.Response.StatusCode = (int)statusCode;

		return context.Response.WriteAsync(result);
	}
}

public sealed class ExceptionDetails
{
	public required string ErrorType { get; init; }
	public required string ErrorMessage { get; init; }
	public required string TraceId { get; init; }
}