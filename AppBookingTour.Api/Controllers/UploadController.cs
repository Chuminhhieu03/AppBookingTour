using AppBookingTour.Api.Contracts.Responses;
using AppBookingTour.Application.IRepositories;
using AppBookingTour.Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace AppBookingTour.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class FilesController : ControllerBase
    {
        private readonly IFileStorageService _fileStorage;
        private readonly IUnitOfWork _unitOfWork;

        public FilesController(IFileStorageService fileStorage, IUnitOfWork unitOfWork)
        {
            _fileStorage = fileStorage;
            _unitOfWork = unitOfWork;
        }

        [HttpPost("upload")]
        public async Task<ActionResult<ApiResponse<string>>> UploadFile(
            IFormFile file,
            //[FromQuery] ImageType imageType,
            [FromQuery] int? entityId = null)
        {
            if (file == null || file.Length == 0)
                return BadRequest(ApiResponse<string>.Fail("No file uploaded"));

            // Validate file type
            var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp" };
            if (!allowedTypes.Contains(file.ContentType))
                return BadRequest(ApiResponse<string>.Fail("Invalid file type"));

            // Upload to cloud
            var fileUrl = await _fileStorage.UploadFileAsync(file.OpenReadStream());

            //// Save to database
            //var image = new Image
            //{
            //    FileName = Path.GetFileName(fileUrl),
            //    OriginalFileName = file.FileName,
            //    Url = fileUrl,
            //    ContentType = file.ContentType,
            //    FileSize = file.Length,
            //    ImageType = imageType,
            //    EntityId = entityId
            //};

            //await _unitOfWork.Repository<Image>().AddAsync(image);
            //await _unitOfWork.SaveChangesAsync();

            return Ok(ApiResponse<string>.Ok(fileUrl));
        }
    }
}
