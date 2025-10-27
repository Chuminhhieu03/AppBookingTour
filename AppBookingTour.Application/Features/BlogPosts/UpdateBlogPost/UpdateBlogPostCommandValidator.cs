using FluentValidation;

namespace AppBookingTour.Application.Features.BlogPosts.UpdateBlogPost;

public class UpdateBlogPostCommandValidator : AbstractValidator<UpdateBlogPostCommand>
{
    public UpdateBlogPostCommandValidator()
    {
        RuleFor(x => x.Request.Id)
            .GreaterThan(0).WithMessage("Id ph?i l?n h?n 0");

        RuleFor(x => x.Request.Title)
            .NotEmpty().WithMessage("Tiêu ?? không ???c ?? tr?ng")
            .MaximumLength(200).WithMessage("Tiêu ?? không ???c v??t quá 200 ký t?")
            .Must(NotContainHtmlTags).WithMessage("Tiêu ?? không ???c ch?a HTML tags");

        RuleFor(x => x.Request.Content)
            .NotEmpty().WithMessage("N?i dung không ???c ?? tr?ng")
            .MaximumLength(100000).WithMessage("N?i dung không ???c v??t quá 100,000 ký t?")
            .Must(NotContainDangerousScripts).WithMessage("N?i dung ch?a mã ??c h?i không ???c phép");

        RuleFor(x => x.Request.Slug)
            .NotEmpty().WithMessage("Slug không ???c ?? tr?ng")
            .MaximumLength(250).WithMessage("Slug không ???c v??t quá 250 ký t?")
            .Matches(@"^[a-z0-9]+(?:-[a-z0-9]+)*$").WithMessage("Slug ch? ???c ch?a ch? th??ng, s? và d?u g?ch ngang");

        RuleFor(x => x.Request.Status)
            .IsInEnum().WithMessage("Tr?ng thái không h?p l?");

        RuleFor(x => x.Request.Tags)
            .MaximumLength(500).WithMessage("Tags không ???c v??t quá 500 ký t?")
            .When(x => !string.IsNullOrEmpty(x.Request.Tags));
    }

    private bool NotContainHtmlTags(string? title)
    {
        if (string.IsNullOrEmpty(title))
            return true;

        return !title.Contains('<') && !title.Contains('>');
    }

    private bool NotContainDangerousScripts(string? content)
    {
        if (string.IsNullOrEmpty(content))
            return true;

        var dangerousPatterns = new[]
        {
            "<script",
            "javascript:",
            "onerror=",
            "onload=",
            "onclick=",
            "onmouseover=",
            "onfocus=",
            "eval(",
            "expression(",
            "vbscript:",
            "data:text/html"
        };

        return !dangerousPatterns.Any(pattern => 
            content.Contains(pattern, StringComparison.OrdinalIgnoreCase));
    }
}
