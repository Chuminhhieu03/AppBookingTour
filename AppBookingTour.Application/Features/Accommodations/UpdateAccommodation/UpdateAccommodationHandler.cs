using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Constants;
using AppBookingTour.Domain.Entities;
using AutoMapper;
using MediatR;

namespace AppBookingTour.Application.Features.Accommodations.UpdateAccommodation
{
    public class UpdateAccommodationHandler : IRequestHandler<UpdateAccommodationCommand, UpdateAccommodationResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateAccommodationHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UpdateAccommodationResponse> Handle(UpdateAccommodationCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Accommodation ?? new UpdateAccommodationDTO();
            var accommodation = await _unitOfWork.Accommodations.GetByIdAsync(request.AccommodationId);
            if (accommodation == null)
                throw new Exception(Message.NotFound);
            _mapper.Map(dto, accommodation);
            _unitOfWork.Accommodations.Update(accommodation);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return new UpdateAccommodationResponse
            {
                Accommodation = accommodation,
                Success = true
            };
        }
    }
}
