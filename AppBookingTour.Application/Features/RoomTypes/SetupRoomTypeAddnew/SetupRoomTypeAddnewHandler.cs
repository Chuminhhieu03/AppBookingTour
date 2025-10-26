using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Constants;
using MediatR;

namespace AppBookingTour.Application.Features.RoomTypes.SetupRoomTypeAddnew
{
    public class SetupRoomTypeAddnewHandler : IRequestHandler<SetupRoomTypeAddnewQuery, SetupRoomTypeAddnewDTO>
    {
        private IUnitOfWork _unitOfWork;
        public SetupRoomTypeAddnewHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<SetupRoomTypeAddnewDTO> Handle(SetupRoomTypeAddnewQuery request, CancellationToken cancellationToken)
        {
            var listCity = await _unitOfWork.Cities.GetAllAsync(cancellationToken);
            return new SetupRoomTypeAddnewDTO
            {
                ListStatus = Constants.RoomTypeStatus.dctName.ToList(),
                Success = true
            };
        }
    }
}
