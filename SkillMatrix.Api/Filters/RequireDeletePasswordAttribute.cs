using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace SkillMatrix.Api.Filters;

// Put on any Delete action (also used for "view password"). The request must carry the delete password in the
// "X-Delete-Password" header (the React app asks for it in a dialog). The password
// lives in appsettings.json ("DeletePassword"), not in the frontend code.
// Also turns "record is still in use" database errors into a readable 409 message.
public class RequireDeletePasswordAttribute : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var config = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
        var expected = config["DeletePassword"] ?? "eaton123";
        var supplied = context.HttpContext.Request.Headers["X-Delete-Password"].ToString();

        var ok = CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(supplied), Encoding.UTF8.GetBytes(expected));

        if (!ok)
        {
            context.Result = new ObjectResult(new { message = "Incorrect password." })
            {
                StatusCode = StatusCodes.Status403Forbidden
            };
            return;
        }

        var executed = await next();
        if (executed.Exception is DbUpdateException)
        {
            executed.ExceptionHandled = true;
            executed.Result = new ObjectResult(new
            {
                message = "This record is used by other records, so it can't be deleted. Remove the linked records first."
            })
            { StatusCode = StatusCodes.Status409Conflict };
        }
    }
}
