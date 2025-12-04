using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.Profiles.GetProfileById;

public sealed class GetProfileByIdQueryHandler : IRequestHandler<GetProfileByIdQuery, GetProfileByIdDTO>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetProfileByIdQueryHandler> _logger;
    private readonly IMapper _mapper;

    public GetProfileByIdQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetProfileByIdQueryHandler> logger,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<GetProfileByIdDTO> Handle(GetProfileByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting user profile details for ID: {UserId}", request.Id);

        var user = await _unitOfWork.Profiles.GetUserByIdAsync(request.Id, cancellationToken);

        if (user == null)
        {
            _logger.LogWarning("User not found with ID: {UserId}", request.Id);
            throw new KeyNotFoundException($"User with ID {request.Id} not found.");
        }
        var bookingCountOfUser = await _unitOfWork.Bookings.CountAsync(b => b.UserId == request.Id, cancellationToken);

        var profileDto = _mapper.Map<GetProfileByIdDTO>(user);
        profileDto.BookingCount = bookingCountOfUser;

        _logger.LogInformation("Successfully retrieved user profile details for ID: {UserId}", request.Id);

        return profileDto;
    }
}