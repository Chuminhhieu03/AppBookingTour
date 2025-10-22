using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using AppBookingTour.Share.DTOS;
using MediatR;
using AutoMapper;

namespace AppBookingTour.Application.Features.RoomInventories.AddNewRoomInventory
{
    public class AddNewRoomInventoryHandler : IRequestHandler<AddNewRoomInventoryCommand, AddNewRoomInventoryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AddNewRoomInventoryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<AddNewRoomInventoryResponse> Handle(AddNewRoomInventoryCommand request, CancellationToken cancellationToken)
        {
            var dto = request.RoomInventory ?? new AddNewRoomInventoryDTO();
            var entity = _mapper.Map<RoomInventory>(dto);
            await _unitOfWork.RoomInventories.AddAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return new AddNewRoomInventoryResponse
            {
                RoomInventory = entity,
                Success = true
            };
        }
    }
}
