using AuthenticationServer.Common.Models.ContractModels;
using AuthenticationServer.Web.Middleware.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;
using System.Net;

namespace AuthenticationServer.Web.Middleware.Filters.Swagger
{
    public class SwaggerProducesFilter : IApplicationModelProvider
    {
        public int Order => 4;

        public void OnProvidersExecuted(ApplicationModelProviderContext context)
        {

        }

        public void OnProvidersExecuting(ApplicationModelProviderContext context)
        {
            foreach (ControllerModel controller in context.Result.Controllers)
            {
                foreach (ActionModel action in controller.Actions)
                {
                    #region Swagger documentation for return type
                    Type returnType;
                    try
                    {
                        returnType = action.ActionMethod.ReturnType.GenericTypeArguments[0].GetGenericArguments()[0];
                    }
                    catch (Exception)
                    {
                        returnType = typeof(void);
                    }
                    Type exceptionType = typeof(ErrorResponse);
                    #endregion

                    #region Swagger documentation for statuscode on success
                    int successActionStatuscode;
                    try
                    {
                        SuccessStatusCode statusAttribute = (SuccessStatusCode)action.ActionMethod.GetCustomAttributes(typeof(SuccessStatusCode), true)[0];
                        successActionStatuscode = statusAttribute.StatusCode;
                    }
                    catch (Exception)
                    {
                        successActionStatuscode = StatusCodes.Status200OK;
                    }
                    #endregion

                    #region Adding the filters
                    action.Filters.Add(new ProducesAttribute("application/json"));
                    action.Filters.Add(new ProducesResponseTypeAttribute(returnType, successActionStatuscode));
                    action.Filters.Add(new ProducesResponseTypeAttribute(exceptionType, StatusCodes.Status400BadRequest));
                    action.Filters.Add(new ProducesResponseTypeAttribute(exceptionType, StatusCodes.Status500InternalServerError));
                    #endregion
                }
            }
        }
    }
}
