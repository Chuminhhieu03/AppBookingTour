using AppBookingTour.Domain.Entities;
using MediatR;

namespace AppBookingTour.Application.Features.Accommodations.GetAccommodationForCustomerById
{
    public record class GetAccommodationForCustomerByIdQuery(int id) : IRequest<GetAccommodationForCustomerByIdDTO>;

}

