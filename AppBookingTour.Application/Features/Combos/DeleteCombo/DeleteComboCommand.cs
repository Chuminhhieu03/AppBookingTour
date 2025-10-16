using MediatR;

namespace AppBookingTour.Application.Features.Combos.DeleteCombo;

public record DeleteComboCommand(int ComboId) : IRequest<DeleteComboResponse>;