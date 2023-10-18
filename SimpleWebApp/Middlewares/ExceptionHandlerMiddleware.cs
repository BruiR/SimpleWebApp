using FluentValidation;
using System.Net;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using Serilog;
using SimpleWebApp.DTOs.Error;
using Microsoft.AspNetCore.Mvc;
using System;
using SimpleWebApp.Domain.Models.Errors;

namespace SimpleWebApp.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next) =>
            _next = next;

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (NotFoundException ex)
            {
                //some logic...
                await HandleExceptionAsync(context, HttpStatusCode.NotFound, ex.Message);
            }
            catch (EntityAlreadyExistException ex)
            {
                //some logic...
                await HandleExceptionAsync(context, HttpStatusCode.BadRequest, ex.Message);
            }
            catch (DuplicationOfEntityRelationships ex)
            {
                //some logic...
                await HandleExceptionAsync(context, HttpStatusCode.Conflict, ex.Message);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                await HandleExceptionAsync(context, HttpStatusCode.InternalServerError, "Internal server error");
            }
        }

        private async Task HandleExceptionAsync(
            HttpContext context,
            HttpStatusCode httpStatusCode,
            string responseMessage)
        {
            HttpResponse response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)httpStatusCode;
            ErrorDto errorDto = new ErrorDto()
            {
                Message = responseMessage,
                StatusCode = (int)httpStatusCode
            };
            string result = JsonSerializer.Serialize(errorDto);

            await response.WriteAsync(result);
        }
     
    }
}
