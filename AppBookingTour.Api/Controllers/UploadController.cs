using AppBookingTour.Api.Contracts.Responses;
using AppBookingTour.Application.IRepositories;
using AppBookingTour.Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace AppBookingTour.Api.Controllers
{
    [ApiController]
    [Route("api/upload")]
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

        [HttpPost("image")]
        public async Task<ActionResult<ApiResponse<string>>> UploadFile(
            IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(ApiResponse<string>.Fail("Không có file nào được chọn để upload"));

            // Validate file type
            var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp" };
            if (!allowedTypes.Contains(file.ContentType))
                return BadRequest(ApiResponse<string>.Fail("Kiểu ảnh không phù hợp"));

            // Upload to cloud
            var fileUrl = await _fileStorage.UploadFileAsync(file.OpenReadStream());

            return Ok(ApiResponse<string>.Ok(fileUrl));
        }
    }
}
