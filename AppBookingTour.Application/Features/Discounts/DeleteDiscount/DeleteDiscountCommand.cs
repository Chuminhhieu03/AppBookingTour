using AppBookingTour.Share.DTOS;
using MediatR;

namespace AppBookingTour.Application.Features.Discounts.DeleteDiscount;

public record DeleteDiscountCommand(int Id) : IRequest<DeleteDiscountResponse>;
