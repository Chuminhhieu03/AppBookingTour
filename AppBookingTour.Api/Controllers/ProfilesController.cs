using AppBookingTour.Api.Contracts.Responses;
using AppBookingTour.Application.Features.Profiles.GetProfileById;
using AppBookingTour.Application.Features.Profiles.UpdateProfile;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AppBookingTour.Api.Controllers;

[Route("api/profiles")]
[ApiController]
public class ProfilesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProfilesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> GetProfile(int id)
    {
        var query = new GetProfileByIdQuery(id);
        var result = await _mediator.Send(query);
        return Ok(ApiResponse<object>.Ok(result));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> UpdateProfile(int id, [FromForm] UpdateProfileDTO requestBody)
    {
        var command = new UpdateProfileCommand(id, requestBody);
        var result = await _mediator.Send(command);
        return Ok(new ApiResponse<object> { Success = true, Message = "Update profile successfully !" });
    }
}