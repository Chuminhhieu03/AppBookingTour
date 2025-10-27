namespace AppBookingTour.Application.IServices;

/// <summary>
/// Service for sanitizing HTML content to prevent XSS attacks
/// </summary>
public interface IHtmlSanitizerService
{
    /// <summary>
    /// Sanitizes HTML content by removing potentially dangerous elements and attributes
    /// </summary>
    /// <param name="htmlContent">Raw HTML content</param>
    /// <returns>Sanitized HTML content</returns>
    string Sanitize(string htmlContent);
}
