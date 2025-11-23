using AppBookingTour.Application.IRepositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.Profiles.GetListGuide;

public sealed class GetListGuideQueryHandler : IRequestHandler<GetListGuideQuery, List<GuideItemDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetListGuideQueryHandler> _logger;
    private readonly IMapper _mapper;
    public GetListGuideQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetListGuideQueryHandler> logger,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }
    public async Task<List<GuideItemDTO>> Handle(GetListGuideQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting list of guides");

        var guides = await _unitOfWork.Profiles.GetGuidesAsync(cancellationToken);
        var guideDtos = _mapper.Map<List<GuideItemDTO>>(guides);

        _logger.LogInformation("Successfully retrieved list of guides");
        return guideDtos;
    }
}
