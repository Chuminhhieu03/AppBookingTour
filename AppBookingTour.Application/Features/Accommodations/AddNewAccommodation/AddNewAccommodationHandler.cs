using AppBookingTour.Application.IRepositories;
using AppBookingTour.Application.IServices;
using AppBookingTour.Domain.Entities;
using AppBookingTour.Share.DTOS;
using AutoMapper;
using MediatR;

namespace AppBookingTour.Application.Features.Accommodations.AddNewAccommodation
{
    public class AddNewAccommodationHandler : IRequestHandler<AddNewAccommodationCommand, AddNewAccommodationResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileStorageService _fileStorageService;

        public AddNewAccommodationHandler(IUnitOfWork unitOfWork, IMapper mapper, IFileStorageService fileStorageService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileStorageService = fileStorageService;
        }

        public async Task<AddNewAccommodationResponse> Handle(AddNewAccommodationCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Accommodation ?? new AddNewAccommodationDTO();
            var accommodation = _mapper.Map<Accommodation>(dto);
            var coverImgFile = dto.CoverImgFile;
            var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp" };
            if (!allowedTypes.Contains(coverImgFile?.ContentType))
                throw new ArgumentException("File tải lên không đúng định dạng jpeg, png, webp");

            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            if (coverImgFile != null)
            {
                var fileUrl = await _fileStorageService.UploadFileAsync(coverImgFile.OpenReadStream());
                accommodation.CoverImgUrl = fileUrl;
            }
            await _unitOfWork.Accommodations.AddAsync(accommodation, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            accommodation.Code = $"CS{accommodation.Id:D5}";
            _unitOfWork.Accommodations.Update(accommodation);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            return new AddNewAccommodationResponse
            {
                Accommodation = accommodation,
                Success = true
            };
        }
    }
}
