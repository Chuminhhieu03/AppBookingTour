using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Constants;
using AppBookingTour.Domain.Entities;
using AutoMapper;
using MediatR;

namespace AppBookingTour.Application.Features.RoomTypes.UpdateRoomType
{
    public class UpdateRoomTypeHandler : IRequestHandler<UpdateRoomTypeCommand, UpdateRoomTypeResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateRoomTypeHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UpdateRoomTypeResponse> Handle(UpdateRoomTypeCommand request, CancellationToken cancellationToken)
        {
            var dto = request.RoomType ?? new UpdateRoomTypeDTO();
            var roomType = await _unitOfWork.RoomTypes.GetByIdAsync(request.RoomTypeId);
            if (roomType == null)
                throw new Exception(Message.NotFound);
            _mapper.Map(dto, roomType);
            _unitOfWork.RoomTypes.Update(roomType);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return new UpdateRoomTypeResponse
            {
                RoomType = roomType,
                Success = true
            };
        }
    }
}
