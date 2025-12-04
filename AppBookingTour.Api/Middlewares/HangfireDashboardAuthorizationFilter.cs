using Hangfire.Dashboard;

namespace AppBookingTour.Api.Middlewares;

/// <summary>
/// Authorization filter cho Hangfire Dashboard
/// Trong môi tr??ng Development: cho phép truy c?p t? do
/// Trong môi tr??ng Production: yêu c?u authentication
/// </summary>
public class HangfireDashboardAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        var httpContext = context.GetHttpContext();
        
        // Trong Development: cho phép t?t c?
        if (httpContext.Request.Host.Host.Contains("localhost"))
        {
            return true;
        }
        
        // Trong Production: ki?m tra authentication
        // TODO: Thêm logic ki?m tra role Admin n?u c?n
        return httpContext.User.Identity?.IsAuthenticated ?? false;
    }
}
