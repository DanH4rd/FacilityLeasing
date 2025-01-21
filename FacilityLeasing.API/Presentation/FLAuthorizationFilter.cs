namespace FacilityLeasing.API.Presentation
{
    /// <summary>
    /// Auth filter to check custom X-AUTH header.
    /// For Azure hosting it's better to configure via a web application firewall (WAF),
    /// for example via Azure Front Door WAF.
    /// </summary>
    public class FLAuthorizationFilter : IEndpointFilter
    {
        private readonly string _authHeader;

        public FLAuthorizationFilter()
        {
            _authHeader = Environment.GetEnvironmentVariable("X_AUTH_HEADER") ?? Guid.NewGuid().ToString();
        }

        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var headerValue = context.HttpContext.Request.Headers["X-AUTH"].FirstOrDefault();

            if (string.IsNullOrWhiteSpace(headerValue) || !_authHeader.Equals(headerValue))
            {
                return Results.Unauthorized();
            }

            return await next(context);
        }
    }
}
