namespace WebApi.Helpers;

using System.Net.Http.Headers;
using System.Text;
using WebApi.Services;

public class BasicAuthenticationMiddleware
{
    private readonly RequestDelegate _next;

    public BasicAuthenticationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        string username, password;
        Console.Out.WriteLine(context.Request.Path);
        if(context.Request.Path.StartsWithSegments("/swagger", StringComparison.OrdinalIgnoreCase))
        {
            await _next(context);
            return;
        }

        try
        {
            var header = AuthenticationHeaderValue.Parse(context.Request.Headers["Authorization"]);
            var credentialBytes = Convert.FromBase64String(header.Parameter);
            var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':', 2);
            username = credentials[0];
            password = credentials[1];

        }
        catch(Exception e)
        {
            throw new AppException("Bad Authorization Credentials format");
        }
        

        if(username != "test" || password != "test")
        {
            throw new UnauthorizedAccessException();
        }
       

        await _next(context);
    }
}
