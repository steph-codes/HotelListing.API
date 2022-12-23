using HotelListing.API.Exceptions;
using Newtonsoft.Json;
using System.Net;

namespace HotelListing.API.Middleware
{
    //Doing All this handling means no need for Try Catch Blocks on the Controllers
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            this._next = next;
            this._logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                    //try the next operation ; this includes everything about the Http Request the potential response etc
                    //below is awaiting the result of the next operation to see if there's any exception thrown

                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went While proccessing in the {context.Request.Path}");
                await HandleExceptionAsync(context, ex);
            }
        }


        //if there is an error we handlemit below and render response in json response
        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            //respond as json
            context.Response.ContentType = "application/json";
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError; // set to 500 by default
            var errorDetails = new ErrorDetails
            {
                ErrorType = "Failure",
                ErrorMessage = ex.Message,
            };

            switch (ex)
            {
                case NotFoundException notFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    errorDetails.ErrorType = "NotFound";
                    break;
                default:
                    break;
            }

            //serialize error Details into JSON response use JsonConvert.SerializeObject(obj)  to decode it you use Deserialize(obj)
            string response = JsonConvert.SerializeObject(errorDetails);
            context.Response.StatusCode = (int)statusCode; //(int converts to integer
            return context.Response.WriteAsync(response);
        }

    }

    public class ErrorDetails
    {
        public string ErrorType { get; set; }
        public string ErrorMessage { get; set; }
    }
}
