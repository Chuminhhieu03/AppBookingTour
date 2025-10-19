using AppBookingTour.Domain.Constants;
using MediatR;

namespace AppBookingTour.Application.Features.Discounts.SetupDiscountDefault
{
    public class SetupDiscountDefaultHandler : IRequestHandler<SetupDiscountDefaultQuery, List<KeyValuePair<int, string>>>
    {
        public Task<List<KeyValuePair<int, string>>> Handle(SetupDiscountDefaultQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(Constants.ActiveStatus.dctName.ToList());
        }
    }
}
