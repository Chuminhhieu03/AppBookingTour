using MediatR;

namespace AppBookingTour.Application.Features.Tours.GetTourById;

/// <summary>
/// Get Tour By ID use case - Query, DTOs, Validator, Handler in one place
/// Following Clean Architecture with integrated DTOs
/// </summary>
public record GetTourByIdQuery(int TourId) : IRequest<GetTourByIdResponse>;
