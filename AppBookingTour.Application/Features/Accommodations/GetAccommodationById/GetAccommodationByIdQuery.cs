using AppBookingTour.Domain.Entities;
using MediatR;

namespace AppBookingTour.Application.Features.Accommodations.GetAccommodationById
{
    public record class GetAccommodationByIdQuery(int id) : IRequest<GetAccommodationByIdDTO>;

}
