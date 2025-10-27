using AppBookingTour.Application.IServices;
using Ganss.Xss;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Infrastructure.Services;

public class HtmlSanitizerService : IHtmlSanitizerService
{
    private readonly HtmlSanitizer _sanitizer;
    private readonly ILogger<HtmlSanitizerService> _logger;

    public HtmlSanitizerService(ILogger<HtmlSanitizerService> logger)
    {
        _logger = logger;
        _sanitizer = new HtmlSanitizer();
        
        ConfigureSanitizer();
    }

    private void ConfigureSanitizer()
    {
        // Clear default and add only allowed tags
        _sanitizer.AllowedTags.Clear();
        
        // Text formatting
        _sanitizer.AllowedTags.Add("p");
        _sanitizer.AllowedTags.Add("br");
        _sanitizer.AllowedTags.Add("strong");
        _sanitizer.AllowedTags.Add("b");
        _sanitizer.AllowedTags.Add("em");
        _sanitizer.AllowedTags.Add("i");
        _sanitizer.AllowedTags.Add("u");
        _sanitizer.AllowedTags.Add("s");
        _sanitizer.AllowedTags.Add("mark");
        
        // Headings
        _sanitizer.AllowedTags.Add("h1");
        _sanitizer.AllowedTags.Add("h2");
        _sanitizer.AllowedTags.Add("h3");
        _sanitizer.AllowedTags.Add("h4");
        _sanitizer.AllowedTags.Add("h5");
        _sanitizer.AllowedTags.Add("h6");
        
        // Lists
        _sanitizer.AllowedTags.Add("ul");
        _sanitizer.AllowedTags.Add("ol");
        _sanitizer.AllowedTags.Add("li");
        
        // Links and media
        _sanitizer.AllowedTags.Add("a");
        _sanitizer.AllowedTags.Add("img");
        
        // Quotes and code
        _sanitizer.AllowedTags.Add("blockquote");
        _sanitizer.AllowedTags.Add("code");
        _sanitizer.AllowedTags.Add("pre");
        
        // Tables
        _sanitizer.AllowedTags.Add("table");
        _sanitizer.AllowedTags.Add("thead");
        _sanitizer.AllowedTags.Add("tbody");
        _sanitizer.AllowedTags.Add("tr");
        _sanitizer.AllowedTags.Add("th");
        _sanitizer.AllowedTags.Add("td");
        
        // Containers
        _sanitizer.AllowedTags.Add("div");
        _sanitizer.AllowedTags.Add("span");
        _sanitizer.AllowedTags.Add("hr");

        // Configure allowed attributes
        _sanitizer.AllowedAttributes.Clear();
        _sanitizer.AllowedAttributes.Add("href");
        _sanitizer.AllowedAttributes.Add("src");
        _sanitizer.AllowedAttributes.Add("alt");
        _sanitizer.AllowedAttributes.Add("title");
        _sanitizer.AllowedAttributes.Add("class");
        _sanitizer.AllowedAttributes.Add("id");
        _sanitizer.AllowedAttributes.Add("target");
        _sanitizer.AllowedAttributes.Add("rel");
        _sanitizer.AllowedAttributes.Add("width");
        _sanitizer.AllowedAttributes.Add("height");
        _sanitizer.AllowedAttributes.Add("align");
        _sanitizer.AllowedAttributes.Add("style"); // Limited style attributes

        // Configure allowed CSS properties
        _sanitizer.AllowedCssProperties.Clear();
        _sanitizer.AllowedCssProperties.Add("color");
        _sanitizer.AllowedCssProperties.Add("background-color");
        _sanitizer.AllowedCssProperties.Add("text-align");
        _sanitizer.AllowedCssProperties.Add("font-size");
        _sanitizer.AllowedCssProperties.Add("font-weight");
        _sanitizer.AllowedCssProperties.Add("padding");
        _sanitizer.AllowedCssProperties.Add("margin");

        // Configure allowed URL schemes
        _sanitizer.AllowedSchemes.Clear();
        _sanitizer.AllowedSchemes.Add("http");
        _sanitizer.AllowedSchemes.Add("https");
        _sanitizer.AllowedSchemes.Add("mailto");

        // Security settings
        _sanitizer.AllowDataAttributes = false;
        _sanitizer.KeepChildNodes = true;
        
        // Remove dangerous event handlers
        _sanitizer.AllowedAttributes.Remove("onclick");
        _sanitizer.AllowedAttributes.Remove("onerror");
        _sanitizer.AllowedAttributes.Remove("onload");
        _sanitizer.AllowedAttributes.Remove("onmouseover");
        _sanitizer.AllowedAttributes.Remove("onfocus");
    }

    public string Sanitize(string htmlContent)
    {
        if (string.IsNullOrWhiteSpace(htmlContent))
        {
            return string.Empty;
        }

        try
        {
            var sanitized = _sanitizer.Sanitize(htmlContent);
            
            _logger.LogDebug("HTML sanitized. Original: {OriginalLength} chars ? Sanitized: {SanitizedLength} chars", 
                htmlContent.Length, sanitized.Length);
            
            return sanitized;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sanitizing HTML content");
            throw new InvalidOperationException("Không th? x? lý n?i dung HTML. Vui lòng ki?m tra l?i ??nh d?ng.", ex);
        }
    }
}
