using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using AppBookingTour.Share.DTOS;
using MediatR;
using AutoMapper;

namespace AppBookingTour.Application.Features.RoomTypes.AddNewRoomType
{
    public class AddNewRoomTypeHandler : IRequestHandler<AddNewRoomTypeCommand, AddNewRoomTypeResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AddNewRoomTypeHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<AddNewRoomTypeResponse> Handle(AddNewRoomTypeCommand request, CancellationToken cancellationToken)
        {
            var dto = request.RoomType ?? new AddNewRoomTypeDTO();
            var roomType = _mapper.Map<RoomType>(dto);
            await _unitOfWork.RoomTypes.AddAsync(roomType, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return new AddNewRoomTypeResponse
            {
                RoomType = roomType,
                Success = true
            };
        }
    }
}
