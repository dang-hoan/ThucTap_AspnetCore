namespace LearnAspNetCoreMVC
{
    public class CustomMiddleWare
    {
        private readonly RequestDelegate _next;

        public CustomMiddleWare(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/private"))
            {
                var username = context.Request.Query["username"];
                if (string.IsNullOrWhiteSpace(username))
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync("<div style=\"font-size: 30px; color: red; font-weight:bold\">You missed the username field!</div>");
                }
                else
                {
                    context.Response.StatusCode = 200;
                    context.Response.ContentType = "text/html";
                    await context.Response.WriteAsync("<body style=\"background-image: url('https://wallpapertag.com/wallpaper/full/d/3/c/968676-hi-res-background-images-2651x1813-retina.jpg'); width:100%; height:100%; background-repeat: no-repeat; background-size: cover; display: flex; justify-content: center; align-items: center;\">" +
                        "                                   <div style=\"font-size: 50px; color: white; font-weight:bold;\">" +
                        "                                       Congratulation you bypassed middleware!" +
                        "                                   </div>" +
                        "                              </body>");
                }
                return;
            }

            // Call the next delegate/middleware in the pipeline.
            await _next(context);
        }
    }

    public static class CustomMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomMiddleWare>();
        }
    }
}
