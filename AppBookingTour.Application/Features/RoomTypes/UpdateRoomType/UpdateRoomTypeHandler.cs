using AppBookingTour.Application.IRepositories;
using AppBookingTour.Application.IServices;
using AppBookingTour.Domain.Constants;
using AppBookingTour.Domain.Entities;
using AutoMapper;
using MediatR;

namespace AppBookingTour.Application.Features.RoomTypes.UpdateRoomType
{
    public class UpdateRoomTypeHandler : IRequestHandler<UpdateRoomTypeCommand, UpdateRoomTypeResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileStorageService _fileStorageService;

        public UpdateRoomTypeHandler(IUnitOfWork unitOfWork, IMapper mapper, IFileStorageService fileStorageService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileStorageService = fileStorageService;
        }

        public async Task<UpdateRoomTypeResponse> Handle(UpdateRoomTypeCommand request, CancellationToken cancellationToken)
        {
            var dto = request.RoomType ?? new UpdateRoomTypeDTO();
            var roomType = await _unitOfWork.RoomTypes.GetByIdAsync(request.RoomTypeId);
            if (roomType == null)
                throw new Exception(Message.NotFound);
            _mapper.Map(dto, roomType);
            
            // Set the new fields if provided
            if (!string.IsNullOrEmpty(dto.CheckinHour) && TimeOnly.TryParse(dto.CheckinHour, out var checkinHour))
                roomType.CheckinHour = checkinHour;
            if (!string.IsNullOrEmpty(dto.CheckoutHour) && TimeOnly.TryParse(dto.CheckoutHour, out var checkoutHour))
                roomType.CheckoutHour = checkoutHour;
            if (dto.Area.HasValue)
                roomType.Area = dto.Area.Value;
            if (dto.View != null)
                roomType.View = dto.View;
            if (dto.CancelPolicy != null)
                roomType.CancelPolicy = dto.CancelPolicy;
            
            var coverImgFile = dto.CoverImgFile;
            var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp" };
            if (string.IsNullOrEmpty(roomType.CoverImageUrl))
                roomType.CoverImageUrl = null;
            if (coverImgFile != null)
            {
                if (!allowedTypes.Contains(coverImgFile?.ContentType))
                    throw new ArgumentException(Message.InvalidImage);
                var fileUrl = await _fileStorageService.UploadFileAsync(coverImgFile.OpenReadStream());
                roomType.CoverImageUrl = fileUrl;
            }

            var listInfoImgOfAccommodation = await _unitOfWork.Images.GetListImageByEntityIdAndEntityType(request.RoomTypeId, Domain.Enums.EntityType.RoomType); // Hiện có
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
                        throw new ArgumentException(Message.InvalidImage);

                    var fileUrl = await _fileStorageService.UploadFileAsync(item.OpenReadStream());
                    var image = new Image
                    {
                        EntityId = roomType.Id,
                        EntityType = Domain.Enums.EntityType.RoomType,
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
            _unitOfWork.RoomTypes.Update(roomType);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return new UpdateRoomTypeResponse
            {
                RoomType = roomType,
                Success = true
            };
        }
    }
}
