using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using AppBookingTour.Share.DTOS;
using MediatR;
using AutoMapper;

namespace AppBookingTour.Application.Features.Accommodations.AddNewAccommodation
{
    public class AddNewAccommodationHandler : IRequestHandler<AddNewAccommodationCommand, AddNewAccommodationResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AddNewAccommodationHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<AddNewAccommodationResponse> Handle(AddNewAccommodationCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Accommodation ?? new AddNewAccommodationDTO();
            var accommodation = _mapper.Map<Accommodation>(dto);
            await _unitOfWork.Accommodations.AddAsync(accommodation, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return new AddNewAccommodationResponse
            {
                Accommodation = accommodation,
                Success = true
            };
        }
    }
}
