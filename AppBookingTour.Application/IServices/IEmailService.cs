namespace AppBookingTour.Application.IServices
{
    public interface IEmailService
    {
        Task SendEmailConfirmAsync(string email, string title, string htmlContent);
    }
}
