using AppBookingTour.Application.IRepositories;
using AppBookingTour.Application.IServices;
using AppBookingTour.Domain.Constants;
using AppBookingTour.Domain.Entities;
using AutoMapper;
using MediatR;
using System.Text.Json;

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
            var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp" };
            if (string.IsNullOrEmpty(accommodation.CoverImgUrl))
                accommodation.CoverImgUrl = null;
            if (coverImgFile != null)
            {
                if (!allowedTypes.Contains(coverImgFile?.ContentType))
                    throw new ArgumentException("File tải lên không đúng định dạng jpeg, png, webp");
                var fileUrl = await _fileStorageService.UploadFileAsync(coverImgFile.OpenReadStream());
                accommodation.CoverImgUrl = fileUrl;
            }

            var listInfoImgOfAccommodation = await _unitOfWork.Images.GetListAccommodationImageByEntityId(request.AccommodationId); // Hiện có
            var ListInfoImageId = dto.ListInfoImageId ?? new List<int>();
            var listInfoImageToDelete = listInfoImgOfAccommodation?
                .Where(x => !ListInfoImageId.Contains(x.Id))
                .ToList();
            var newListInfoImg = dto.ListNewInfoImage;
            var listImage = new List<Image>();
            if (newListInfoImg != null)
            {
                foreach (var item in newListInfoImg)
                {
                    if (!allowedTypes.Contains(item?.ContentType))
                        throw new ArgumentException("File tải lên không đúng định dạng jpeg, png, webp");

                    var fileUrl = await _fileStorageService.UploadFileAsync(item.OpenReadStream());
                    var image = new Image
                    {
                        EntityId = accommodation.Id,
                        EntityType = Domain.Enums.EntityType.Accommodation,
                        Url = fileUrl
                    };
                    listImage.Add(image);
                }
            }

            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            if (listInfoImageToDelete != null && listInfoImageToDelete.Count > 0)
            {
                foreach (var img in listInfoImageToDelete)
                    await _fileStorageService.DeleteFileAsync(img.Url);
                _unitOfWork.Images.RemoveRange(listInfoImageToDelete);
            }
            await _unitOfWork.Images.AddRangeAsync(listImage);
            _unitOfWork.Accommodations.Update(accommodation);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return new UpdateAccommodationResponse
            {
                Accommodation = accommodation,
                Success = true
            };
        }
    }
}
