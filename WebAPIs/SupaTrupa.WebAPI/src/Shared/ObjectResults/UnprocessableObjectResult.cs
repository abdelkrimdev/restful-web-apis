using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SupaTrupa.WebAPI.Shared.ObjectResults
{
    /// <summary>
    /// A custom ObjectResult with 422 (UnprocessableEntity) Status Code.
    /// </summary>
    public class UnprocessableObjectResult : ObjectResult
    {        
        /// <summary>
        /// Initializes a new instance of the UnprocessableObjectResult class.
        /// </summary>
        /// <param name="modelState">validation errors which will be returned to the client.</param>
        public UnprocessableObjectResult(ModelStateDictionary modelState)
            : base(new SerializableError(modelState))
        {
            StatusCode = StatusCodes.Status422UnprocessableEntity;
        }
    }
}
