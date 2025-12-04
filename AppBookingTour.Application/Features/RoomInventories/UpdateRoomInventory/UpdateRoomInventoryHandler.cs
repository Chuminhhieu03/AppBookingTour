using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Constants;
using AppBookingTour.Domain.Entities;
using AutoMapper;
using MediatR;

namespace AppBookingTour.Application.Features.RoomInventories.UpdateRoomInventory
{
    public class UpdateRoomInventoryHandler : IRequestHandler<UpdateRoomInventoryCommand, UpdateRoomInventoryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UpdateRoomInventoryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<UpdateRoomInventoryResponse> Handle(UpdateRoomInventoryCommand request, CancellationToken cancellationToken)
        {
            var dto = request.RoomInventory ?? new UpdateRoomInventoryDTO();
            var entity = await _unitOfWork.RoomInventories.GetByIdAsync(request.RoomInventoryId);
            if (entity == null)
                throw new Exception(Message.NotFound);
            _mapper.Map(dto, entity);
            _unitOfWork.RoomInventories.Update(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return new UpdateRoomInventoryResponse
            {
                RoomInventory = entity,
                Success = true
            };
        }
    }
}
