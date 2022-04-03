using AuthenticationServer.Common.Exceptions;
using AuthenticationServer.Common.Models.ContractModels;
using AuthenticationServer.Common.Models.ContractModels.Account;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace AuthenticationServer.Web.Middleware.Filters
{
    public class ErrorFilter : IAsyncExceptionFilter
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<ErrorFilter> _logger;

        public ErrorFilter(IWebHostEnvironment environment, ILogger<ErrorFilter> logger)
        {
            _environment = environment;
            _logger = logger;
        }

        public async Task OnExceptionAsync(ExceptionContext context)
        {
            await Task.Run(() =>
            {
                _logger.LogError(context.Exception.Message);
                var errorResponse = new ErrorResponse();

                if (typeof(AuthenticationApiException) == context.Exception.GetType())
                {
                    AuthenticationApiException apiException = (AuthenticationApiException)context.Exception;

                    context.HttpContext.Response.StatusCode = apiException.StatusCode;
                    errorResponse.Errors = apiException.Errors;
                }

                else if (typeof(Exception) == context.Exception.GetType())
                {
                    context.HttpContext.Response.StatusCode = 500;
                    string message = "Something went terrible wrong!";
                    if (_environment.IsDevelopment())
                        message = context.Exception.Message;

                    errorResponse.Errors.Add(new ErrorModel()
                    {
                        FieldName = "Internal Server Error",
                        Message = message
                    });

                }
                else
                {
                    context.HttpContext.Response.StatusCode = 500;
                    string message = "OMFG, what did you do to me!?";
                    if (_environment.IsProduction() == false)
                        message = context.Exception.ToString();

                    errorResponse.Errors.Add(new ErrorModel()
                    {
                        FieldName = "Internal Server Error",
                        Message = message
                    });
                }

                context.Result = new BadRequestObjectResult(errorResponse);
            });
        }
    }
}
