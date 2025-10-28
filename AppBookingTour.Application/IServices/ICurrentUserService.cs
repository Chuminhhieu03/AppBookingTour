namespace AppBookingTour.Application.IServices;

/// <summary>
/// Service for retrieving current authenticated user information from JWT token
/// </summary>
public interface ICurrentUserService
{
    /// <summary>
    /// Get current user ID from JWT claims
    /// </summary>
    /// <returns>User ID if authenticated, null otherwise</returns>
    int? GetCurrentUserId();

    /// <summary>
    /// Get current user email from JWT claims
    /// </summary>
    /// <returns>User email if authenticated, null otherwise</returns>
    string? GetCurrentUserEmail();

    /// <summary>
    /// Get current user name from JWT claims
    /// </summary>
    /// <returns>User name if authenticated, null otherwise</returns>
    string? GetCurrentUserName();

    /// <summary>
    /// Check if current user is authenticated
    /// </summary>
    /// <returns>True if user is authenticated, false otherwise</returns>
    bool IsAuthenticated();
}
