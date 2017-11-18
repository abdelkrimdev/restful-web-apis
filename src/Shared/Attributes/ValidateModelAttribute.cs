using Microsoft.AspNetCore.Mvc.Filters;

using SupaTrupa.WebAPI.Shared.ObjectResults;

namespace SupaTrupa.WebAPI.Shared.Attributes
{
    /// <summary>
    /// Attribute used to validate input models.
    /// A status code of 422 (Unprocessable Entity) can be returned to indicate validation failure to client.
    /// </summary>
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Check if the ModelState is valid, and if not it returns a status code of 422.
        /// </summary>
        /// <param name="context">The request's context object.</param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new UnprocessableObjectResult(context.ModelState);
            }
        }
    }
}
