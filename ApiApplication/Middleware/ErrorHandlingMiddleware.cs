using ApiApplication.Domain.Exceptions;

using Microsoft.AspNetCore.Http;

using Newtonsoft.Json;

using System;
using System.Net;
using System.Threading.Tasks;

namespace ApiApplication.Middleware {
    internal class ErrorHandlingMiddleware {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next) {
            _next = next;
        }

        public async Task Invoke(HttpContext context) {
            try {
                await _next(context);
            } catch (EntityNotFoundException ex) {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                await HandleExceptionAsync(context, ex);
            } catch (ReservationException ex) {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await HandleExceptionAsync(context, ex);
            } catch (PaymentException ex) {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await HandleExceptionAsync(context, ex);
            } catch (DomainException ex) {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await HandleExceptionAsync(context, ex);
            } catch (Exception ex) {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception) {
            context.Response.ContentType = "application/json";

            return context.Response.WriteAsync(JsonConvert.SerializeObject(new {
                error = new {
                    message = "An error occurred while processing your request.",
                    details = exception.Message
                }
            }));
        }
    }
}
