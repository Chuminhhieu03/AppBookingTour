using AppBookingTour.Share.DTOS;
using MediatR;

namespace AppBookingTour.Application.Features.Combos.GetListCombos;

public record GetListCombosQuery(GetListCombosRequest Request) : IRequest<PagedResult<ComboListDTO>>;
