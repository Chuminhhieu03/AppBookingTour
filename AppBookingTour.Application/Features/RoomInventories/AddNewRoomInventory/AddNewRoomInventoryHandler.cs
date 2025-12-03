using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using AutoMapper;
using MediatR;

namespace AppBookingTour.Application.Features.RoomInventories.AddNewRoomInventory
{
    public class AddNewRoomInventoryHandler
        : IRequestHandler<AddNewRoomInventoryCommand, AddNewRoomInventoryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddNewRoomInventoryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AddNewRoomInventoryResponse> Handle(
            AddNewRoomInventoryCommand request,
            CancellationToken cancellationToken)
        {
            if (request.RoomInventory is null)
            {
                return new AddNewRoomInventoryResponse
                {
                    Success = false,
                    Message = "Thiếu dữ liệu room inventory."
                };
            }

            var entity = _mapper.Map<RoomInventory>(request.RoomInventory);

            if (entity.BasePriceAdult <= 0)
            {
                entity.BasePriceAdult = entity.BasePrice;
            }

            if (entity.BasePriceChildren <= 0)
            {
                entity.BasePriceChildren = entity.BasePrice;
            }

            await _unitOfWork.RoomInventories.AddAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new AddNewRoomInventoryResponse
            {
                Success = true,
                RoomInventory = entity,
                Message = "Tạo room inventory thành công."
            };
        }
    }
}


