using MediatR;

namespace AppBookingTour.Application.Features.Combos.CreateCombo;

public record CreateComboCommand(ComboRequestDTO ComboRequest) : IRequest<CreateComboResponse>;