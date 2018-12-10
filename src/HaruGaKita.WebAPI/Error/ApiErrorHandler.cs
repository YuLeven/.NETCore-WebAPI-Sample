using System;
using System.Reflection;
using System.Threading.Tasks;
using HaruGaKita.Application.Exceptions;
using HaruGaKita.Common.Postgres;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Npgsql;

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
            else if (_exception is DbUpdateException)
            {
                HandleSLQFailure();
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

        private void HandleSLQFailure()
        {
            if (_exception.InnerException is PostgresException)
            {
                var innerException = (PostgresException)_exception.InnerException;
                switch (innerException.SqlState)
                {
                    case "23505":
                        _problemDetails.Status = 400;
                        _problemDetails.Title = "Uniqueness violation";
                        _problemDetails.Detail = new SqlErrorMessageFormatter(innerException.Detail).Format();
                        break;
                    default:
                        _problemDetails.Status = 500;
                        _problemDetails.Title = "Database problem";
                        _problemDetails.Detail = "The database rejected the requeste changes";
                        break;
                }
            }
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