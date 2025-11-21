using MediatR;
using Microsoft.AspNetCore.Mvc;
using AppBookingTour.Api.Contracts.Responses;
using AppBookingTour.Application.Features.Combos.CreateCombo;
using AppBookingTour.Application.Features.Combos.GetComboById;
using AppBookingTour.Application.Features.Combos.GetFeaturedCombos;
using AppBookingTour.Application.Features.Combos.UpdateCombo;
using AppBookingTour.Application.Features.Combos.DeleteCombo;
using AppBookingTour.Application.Features.Combos.GetListCombos;
using AppBookingTour.Application.Features.Combos.SearchCombosForCustomer;
using AppBookingTour.Application.Features.Combos.UploadComboImages;
using AppBookingTour.Application.Features.Combos.DeleteComboCoverImage;
using AppBookingTour.Application.Features.Combos.DeleteComboGalleryImages;
using AppBookingTour.Share.DTOS;

namespace AppBookingTour.Api.Controllers;

[ApiController]
[Route("api/combos")]
public sealed class CombosController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<CombosController> _logger;

    public CombosController(IMediator mediator, ILogger<CombosController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost]
    //[Authorize(Roles = "Admin,Staff")]
    public async Task<ActionResult<ApiResponse<object>>> CreateCombo([FromBody] ComboRequestDTO requestBody)
    {
        var command = new CreateComboCommand(requestBody);
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            return BadRequest(ApiResponse<object>.Fail(result.ErrorMessage!));
        }

        _logger.LogInformation("Created new combo with ID: {ComboId}", result.Combo!.Id);
        return CreatedAtAction(nameof(GetComboById), new { id = result.Combo!.Id }, ApiResponse<object>.Ok(result.Combo!));
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResult<ComboListDTO>>>> GetListCombos([FromQuery] GetListCombosRequest request)
    {
        var query = new GetListCombosQuery(request);
        var result = await _mediator.Send(query);

        _logger.LogInformation("Retrieved {Count} combos", result.Items.Count);
        return Ok(ApiResponse<PagedResult<ComboListDTO>>.Ok(result));
    }

    [HttpPost("search-for-customer")]
    public async Task<ActionResult<ApiResponse<object>>> SearchComboForCustomer([FromBody] SearchCombosForCustomerQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(ApiResponse<object>.Ok(result));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> GetComboById(int id)
    {
        var query = new GetComboByIdQuery(id);
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
        {
            if (result.ErrorMessage?.Contains("not found") == true)
            {
                return NotFound(ApiResponse<object>.Fail(result.ErrorMessage!));
            }
            return BadRequest(ApiResponse<object>.Fail(result.ErrorMessage!));
        }

        _logger.LogInformation("Retrieved combo details for ID: {ComboId}", id);
        return Ok(ApiResponse<object>.Ok(result.Combo!));
    }

    [HttpPut("{id:int}")]
    //[Authorize(Roles = "Admin,Staff")]
    public async Task<ActionResult<ApiResponse<object>>> UpdateCombo(int id, [FromBody] ComboRequestDTO requestBody)
    {
        var command = new UpdateComboCommand(id, requestBody);
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            if (result.ErrorMessage?.Contains("not found") == true || result.ErrorMessage?.Contains("không tồn tại") == true)
            {
                return NotFound(ApiResponse<object>.Fail(result.ErrorMessage!));
            }
            return BadRequest(ApiResponse<object>.Fail(result.ErrorMessage!));
        }

        _logger.LogInformation("Updated combo with ID {ComboId}", id);
        return Ok(new ApiResponse<object> { Success = true, Message = "Update combo successfully" });
    }

    [HttpDelete("{id:int}")]
    //[Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<object>>> DeleteCombo(int id)
    {
        var command = new DeleteComboCommand(id);
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            if (result.ErrorMessage?.Contains("not found") == true || result.ErrorMessage?.Contains("không tồn tại") == true)
            {
                return NotFound(ApiResponse<object>.Fail(result.ErrorMessage!));
            }
            return BadRequest(ApiResponse<object>.Fail(result.ErrorMessage!));
        }

        _logger.LogInformation("Deleted combo with ID {ComboId}", id);
        return Ok(new ApiResponse<object> { Success = true, Message = "Delete combo successfully" });
    }

    /// <summary>
    /// Upload images for combo
    /// </summary>
    /// <param name="id">Combo ID</param>
    /// <param name="coverImage">Cover image (optional, max 5MB)</param>
    /// <param name="images">Additional images (optional, max 10 images, each max 5MB)</param>
    [HttpPost("{id:int}/upload-images")]
    //[Authorize(Roles = "Admin,Staff")]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<ApiResponse<UploadComboImagesResponse>>> UploadComboImages(
        UploadComboImagesForm input)
    {
        var command = new UploadComboImagesCommand(input.Id, input.CoverImage, input.Images);
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            if (result.ErrorMessage?.Contains("không tồn tại") == true)
            {
                return NotFound(ApiResponse<UploadComboImagesResponse>.Fail(result.ErrorMessage));
            }
            return BadRequest(ApiResponse<UploadComboImagesResponse>.Fail(result.ErrorMessage!));
        }

        _logger.LogInformation("Uploaded images for combo {ComboId}: Cover={HasCover}, Count={ImageCount}", 
            input.Id, result.CoverImageUrl != null, result.ImageUrls.Count);
        
        return Ok(ApiResponse<UploadComboImagesResponse>.Ok(result));
    }

    /// <summary>
    /// Delete cover image for combo
    /// </summary>
    /// <param name="id">Combo ID</param>
    [HttpDelete("{id:int}/cover-image")]
    //[Authorize(Roles = "Admin,Staff")]
    public async Task<ActionResult<ApiResponse<object>>> DeleteCoverImage(int id)
    {
        var command = new DeleteComboCoverImageCommand(id);
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            if (result.ErrorMessage?.Contains("không tồn tại") == true)
            {
                return NotFound(ApiResponse<object>.Fail(result.ErrorMessage));
            }
            return BadRequest(ApiResponse<object>.Fail(result.ErrorMessage!));
        }

        _logger.LogInformation("Deleted cover image for combo {ComboId}", id);
        return Ok(new ApiResponse<object> { Success = true, Message = "Đã xóa ảnh bìa thành công" });
    }

    /// <summary>
    /// Delete gallery images for combo
    /// </summary>
    /// <param name="id">Combo ID</param>
    /// <param name="request">List of image URLs to delete</param>
    [HttpDelete("{id:int}/gallery-images")]
    //[Authorize(Roles = "Admin,Staff")]
    public async Task<ActionResult<ApiResponse<object>>> DeleteGalleryImages(
        int id, 
        [FromBody] DeleteComboGalleryImagesRequest request)
    {
        var command = new DeleteComboGalleryImagesCommand(id, request.ImageUrls);
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            if (result.ErrorMessage?.Contains("không tồn tại") == true)
            {
                return NotFound(ApiResponse<object>.Fail(result.ErrorMessage));
            }
            return BadRequest(ApiResponse<object>.Fail(result.ErrorMessage!));
        }

        _logger.LogInformation("Deleted {Count} gallery images for combo {ComboId}", result.DeletedCount, id);
        return Ok(new ApiResponse<object> 
        { 
            Success = true, 
            Message = $"Đã xóa {result.DeletedCount} ảnh thành công" 
        });
    }

    /// <summary>
    /// Get featured combos (random selection)
    /// </summary>
    /// <param name="count">Number of combos to retrieve (default: 6, max: 50)</param>
    [HttpGet("featured")]
    public async Task<ActionResult<ApiResponse<List<FeaturedComboDTO>>>> GetFeaturedCombos([FromQuery] int count = 6)
    {
        if (count <= 0 || count > 50)
        {
            return BadRequest(ApiResponse<List<FeaturedComboDTO>>.Fail("Số lượng phải từ 1 đến 50"));
        }

        var query = new GetFeaturedCombosQuery(count);
        var result = await _mediator.Send(query);

        _logger.LogInformation("Retrieved {Count} featured combos", result.Count);
        return Ok(ApiResponse<List<FeaturedComboDTO>>.Ok(result));
    }
}
