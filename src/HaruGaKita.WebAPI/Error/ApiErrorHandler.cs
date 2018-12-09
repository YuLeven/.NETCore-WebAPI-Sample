using System;
using System.Reflection;
using System.Threading.Tasks;
using HaruGaKita.Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Newtonsoft.Json;

#pragma warning disable 1591
namespace HaruGaKita.WebAPI.Error
{
    public class ApiErrorHandler
    {
        private readonly HttpContext _context;
        private readonly Exception _exception;
        private readonly ProblemDetails _problemDetails;

        public ApiErrorHandler(HttpContext context)
        {
            _context = context;
            _exception = _context.Features.Get<IExceptionHandlerFeature>().Error;
            _problemDetails = new ProblemDetails
            {
                Instance = $"urn:harugakita:error:{Guid.NewGuid()}",
                Detail = _exception.Message
            };
        }

        public void Handle()
        {
            if (_exception is BadHttpRequestException)
            {
                HandleBadRequest();
            }
            else if (_exception is UnauthenticatedException)
            {
                HandleUnauthentictedUser();
            }
            else
            {
                _problemDetails.Title = "An unexpected error occurred!";
                _problemDetails.Status = 500;
            }

            _context.Response.StatusCode = _problemDetails.Status.Value;
            _context.Response.WriteJson(_problemDetails);
        }

        private void HandleBadRequest()
        {
            _problemDetails.Title = "Invalid request";
            _problemDetails.Status = (int)typeof(BadHttpRequestException).GetProperty("StatusCode",
                BindingFlags.NonPublic | BindingFlags.Instance).GetValue(_exception);
        }

        private void HandleUnauthentictedUser()
        {
            _problemDetails.Title = "Unauthorized";
            _problemDetails.Status = 401;
        }
    }
}