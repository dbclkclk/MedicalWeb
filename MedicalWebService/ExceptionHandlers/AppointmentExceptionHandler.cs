using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;

namespace MedicalWebService.ExceptionHandlers
{
    public class AppointmentExceptionHandler : ExceptionFilterAttribute
    {
            public override void OnException(HttpActionExecutedContext context)
            {
                if (context.Exception is UnauthorizedAccessException)
                {
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden)
                    {
                        Content = new StringContent(context.Exception.Message),
                        ReasonPhrase = "Exception"
                    });
                }
                if (context.Exception is KeyNotFoundException)
                {
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Conflict)
                    {
                        Content = new StringContent(context.Exception.Message),
                        ReasonPhrase = "Exception"
                    });
                }
                if (context.Exception is InvalidOperationException)
                {
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent(context.Exception.Message),
                        ReasonPhrase = "Exception"
                    });
                }
                if (context.Exception is DbEntityValidationException)
                {
                    var errors = new List<string>();
                    foreach (var e in ((DbEntityValidationException)context.Exception).EntityValidationErrors)
                    {
                        errors.AddRange(e.ValidationErrors.Select(e2 => string.Join("Validation Error :: ", e2.PropertyName, " : ", e2.ErrorMessage)));
                    }
                    var error = string.Join("<br />", errors);
                
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent(error),
                        ReasonPhrase = "Exception"
                    });
                }
            if (context.Exception is DbUpdateConcurrencyException)
            {

                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Conflict)
                {
                    Content = new StringContent("Someone has updated this record and the record you're updating is stall. Try navigating back to the list and selecting the new record for updating"),
                    ReasonPhrase = "Exception"
                });
            }

            
            base.OnException(context);
        }
        
    }
}