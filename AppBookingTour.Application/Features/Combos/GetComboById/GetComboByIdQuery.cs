using MediatR;

namespace AppBookingTour.Application.Features.Combos.GetComboById;

public record GetComboByIdQuery(int ComboId) : IRequest<GetComboByIdResponse>;