


namespace logiWeb.Middlewares; 

public class AuthenticationMiddleware {
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public AuthenticationMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
    {
        _next = next;
        _logger = loggerFactory.CreateLogger<AuthenticationMiddleware>();
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
          
           List<string> privateRoute = new List<string>{"/ordem","/funcionario","/cliente"};

           if(privateRoute.Contains(context.Request.Path.ToString()) && context.Session.Get("Auth") == null)
               context.Response.Redirect("/");


         await _next(context);
        }
        finally
        {
           /* _logger.LogInformation(
                "Request {method} {url} => {statusCode}",
                context.Request?.Method,
                context.Request?.Path.Value,
                context.Response?.StatusCode); */
        }
    }
}