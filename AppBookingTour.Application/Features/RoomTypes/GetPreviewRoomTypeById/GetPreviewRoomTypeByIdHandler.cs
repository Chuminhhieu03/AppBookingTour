using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Constants;
using AppBookingTour.Domain.Entities;
using MediatR;

namespace AppBookingTour.Application.Features.RoomTypes.GetPreviewRoomTypeById
{
    public class GetPreviewRoomTypeByIdHandler : IRequestHandler<GetPreviewRoomTypeByIdQuery, PreviewRoomTypeDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetPreviewRoomTypeByIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<PreviewRoomTypeDTO> Handle(GetPreviewRoomTypeByIdQuery request, CancellationToken cancellationToken)
        {
            var roomType = await _unitOfWork.RoomTypes.GetById(request.Id);
            if (roomType == null)
            {
                throw new Exception(Message.NotFound);
            }
            // Lấy accommodation liên quan
            var accommodation = await _unitOfWork.Accommodations.GetById(roomType.AccommodationId);
            return new PreviewRoomTypeDTO
            {
                RoomType = roomType,
                Accommodation = accommodation,
                Success = true
            };
        }
    }
}
