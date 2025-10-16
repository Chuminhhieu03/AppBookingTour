using MediatR;

using AppBookingTour.Application.Features.Combos.CreateCombo;

namespace AppBookingTour.Application.Features.Combos.UpdateCombo;

public record UpdateComboCommand(int ComboId, ComboRequestDTO ComboRequest) : IRequest<UpdateComboResponse>;