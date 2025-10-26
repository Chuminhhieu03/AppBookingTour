using AppBookingTour.Application.IRepositories;
using AppBookingTour.Application.IServices;
using AppBookingTour.Domain.Constants;
using AppBookingTour.Domain.Entities;
using AppBookingTour.Share.DTOS;
using AutoMapper;
using MediatR;

namespace AppBookingTour.Application.Features.RoomTypes.AddNewRoomType
{
    public class AddNewRoomTypeHandler : IRequestHandler<AddNewRoomTypeCommand, AddNewRoomTypeResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileStorageService _fileStorageService;

        public AddNewRoomTypeHandler(IUnitOfWork unitOfWork, IMapper mapper, IFileStorageService fileStorageService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileStorageService = fileStorageService;
        }
        public async Task<AddNewRoomTypeResponse> Handle(AddNewRoomTypeCommand request, CancellationToken cancellationToken)
        {
            var dto = request.RoomType ?? new AddNewRoomTypeDTO();
            var roomType = _mapper.Map<RoomType>(dto);

            var coverImgFile = dto.CoverImgFile;
            var infoImgFile = dto.InfoImgFile;

            var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp" };

            if (coverImgFile != null)
            {
                if (!allowedTypes.Contains(coverImgFile?.ContentType))
                    throw new ArgumentException(Message.InvalidImage);
                var fileUrl = await _fileStorageService.UploadFileAsync(coverImgFile.OpenReadStream());
                roomType.CoverImageUrl = fileUrl;
            }
            var listImage = new List<Image>();

            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            await _unitOfWork.RoomTypes.AddAsync(roomType, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            if (infoImgFile != null)
            {
                foreach (var item in infoImgFile)
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
            await _unitOfWork.Images.AddRangeAsync(listImage);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return new AddNewRoomTypeResponse
            {
                RoomType = roomType,
                Success = true
            };
        }
    }
}
