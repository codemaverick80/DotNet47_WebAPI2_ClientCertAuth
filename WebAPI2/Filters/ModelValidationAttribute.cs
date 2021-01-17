using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using WebAPI2.Models;

namespace WebAPI2.Filters
{
    public class ModelValidationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            string message = string.Empty;
            bool firstError = true;
            
            string actionName = actionContext.ActionDescriptor.ActionName;
            string controllerName = actionContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string stream = string.Empty;


            #region "Request Header Validation"

            HeaderModel headerModel = HeaderUtilityModel.GetHeaders(actionContext.Request.Headers);
            ValidationContext validateHeaderModel = new ValidationContext(headerModel);
            var validationResults = new List<ValidationResult>();
            bool isHeaderValid = Validator.TryValidateObject(headerModel, validateHeaderModel,validationResults,true);
            if (!isHeaderValid)
            {
                ApiErrorResponse er = new ApiErrorResponse();
                er.SystemError = new List<SystemError>();
                foreach (var item in validationResults)
                {
                    SystemError se = new SystemError()
                    {
                        CreatorApplicatioId = "WEB-API", // Web Api Id (name)
                        Code = "invalid Headers",
                        Message = item.ErrorMessage
                    };
                    er.SystemError.Add(se);
                }

                var response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, er);
                actionContext.Response = response;
                return;
            }

            #endregion

            #region "Request Model Validation"

            if (actionContext.ModelState.IsValid == false)
            {
                if (actionContext.ActionDescriptor.ActionName == "SaveProduct")
                {
                    ApiErrorResponse er = new ApiErrorResponse();
                    er.SystemError = new List<SystemError>();

                    foreach (var item in actionContext.ModelState.Values)
                    {
                        foreach (var e in item.Errors)
                        {
                            SystemError se = new SystemError()
                            {
                                CreatorApplicatioId = "WEB-API",
                                Code = "Invalid product post request",
                                Message = string.IsNullOrEmpty(e.ErrorMessage) ? e.Exception.Message : e.ErrorMessage
                            };
                            er.SystemError.Add(se);
                        }

                    }

                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, er);
                }
                else
                {
                    foreach (var item in actionContext.ModelState.Values)
                    {
                        foreach (var e in item.Errors)
                        {
                            if (!firstError)
                            {
                                message += "; ";
                            }
                            message += string.IsNullOrEmpty(e.ErrorMessage) ? e.Exception.Message : e.ErrorMessage;
                            firstError = false;
                        }
                    }

                    ApiRequestErrorModel error = new ApiRequestErrorModel();
                    error.ErrorMessage = message;
                    error.ErrotType = typeof(ApiRequiredFieldException).Name;

                    var response = new ApiResponse<object>()
                    {
                        Status = HttpStatusCode.BadRequest,
                        Data = null,
                        Error = error
                    };

                    actionContext.Response = actionContext.Request.CreateResponse(response);
                }
            }



            #endregion


        }






        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);

            if(actionExecutedContext.ActionContext.Response!=null)
            {
                HeaderModel responseHeader = new HeaderModel().ResponseHeader(HeaderUtilityModel.GetHeaders(actionExecutedContext.Request.Headers));
                actionExecutedContext.ActionContext.Response.Headers.Add("API-CreationTimeStamp", responseHeader.CreationTimeStamp == 
                    DateTime.MinValue ? null : responseHeader.CreationTimeStamp.ToString("yyyy-MM-ddTHH:mm:ss.fffK"));

                actionExecutedContext.ActionContext.Response.Headers.Add("API-SenderHostName", responseHeader.SenderHostName);
                actionExecutedContext.ActionContext.Response.Headers.Add("API-SenderMessageIdEcho", responseHeader.SenderMessageIdEcho);
                actionExecutedContext.ActionContext.Response.Headers.Add("API-SenderMessageId", responseHeader.SenderMessageId);
                actionExecutedContext.ActionContext.Response.Headers.Add("API-SenderApplicationId", responseHeader.SenderApplicationId);
                actionExecutedContext.ActionContext.Response.Headers.Add("API-TransactionId", responseHeader.TransactionId);
                actionExecutedContext.ActionContext.Response.Headers.Add("API-OriginationApplicationId", responseHeader.OriginationApplicationId);

                string actionName = actionExecutedContext.ActionContext.ActionDescriptor.ActionName;
                string controllerName = actionExecutedContext.ActionContext.ActionDescriptor.ControllerDescriptor.ControllerName;

                string stream = string.Empty;
                try
                {
                    if (actionExecutedContext.Response.Content != null)
                    {
                        var responseBody = actionExecutedContext.Response.Content.ReadAsStringAsync();
                        stream = responseBody.Result;
                    }

                    //// Log response to DB or certerlize logging location 

                }
                catch{}
            }
        }
    }
}