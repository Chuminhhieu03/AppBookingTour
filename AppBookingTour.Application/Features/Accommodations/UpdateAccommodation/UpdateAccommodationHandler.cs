using AppBookingTour.Application.IRepositories;
using AppBookingTour.Application.IServices;
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
        private readonly IFileStorageService _fileStorageService;

        public UpdateAccommodationHandler(IUnitOfWork unitOfWork, IMapper mapper, IFileStorageService fileStorageService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileStorageService = fileStorageService;
        }

        public async Task<UpdateAccommodationResponse> Handle(UpdateAccommodationCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Accommodation ?? new UpdateAccommodationDTO();
            var accommodation = await _unitOfWork.Accommodations.GetByIdAsync(request.AccommodationId);
            if (accommodation == null)
                throw new Exception(Message.NotFound);
            _mapper.Map(dto, accommodation);
            var coverImgFile = dto.CoverImgFile;
            if (coverImgFile != null)
            {
                var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp" };
                if (!allowedTypes.Contains(coverImgFile?.ContentType))
                    throw new ArgumentException("File tải lên không đúng định dạng jpeg, png, webp");
                var fileUrl = await _fileStorageService.UploadFileAsync(coverImgFile.OpenReadStream());
                accommodation.CoverImgUrl = fileUrl;
            }
            else
            {
                accommodation.CoverImgUrl = null;
            }
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
