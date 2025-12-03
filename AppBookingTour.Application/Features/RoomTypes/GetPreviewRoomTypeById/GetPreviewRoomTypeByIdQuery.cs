using MediatR;

namespace AppBookingTour.Application.Features.RoomTypes.GetPreviewRoomTypeById
{
    public record GetPreviewRoomTypeByIdQuery(int Id) : IRequest<PreviewRoomTypeDTO>;
}
