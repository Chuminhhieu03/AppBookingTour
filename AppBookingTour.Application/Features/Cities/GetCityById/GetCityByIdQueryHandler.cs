using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System.ComponentModel;

namespace AppBookingTour.Application.Features.Cities.GetCityById;

public sealed class GetCityByIdQueryHandler : IRequestHandler<GetCityByIdQuery, GetCityByIdResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetCityByIdQueryHandler> _logger;
    private readonly IMapper _mapper;

    public GetCityByIdQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetCityByIdQueryHandler> logger,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<GetCityByIdResponse> Handle(GetCityByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting city details for ID: {CityId}", request.CityId);

        try
        {
            var city = await _unitOfWork.Repository<City>().GetByIdAsync(request.CityId, cancellationToken);

            if (city == null)
            {
                _logger.LogWarning("City not found with ID: {CityId}", request.CityId);
                return GetCityByIdResponse.Failed($"City with ID {request.CityId} not found");
            }

            var cityDto = _mapper.Map<CityDTO>(city);

            if (city.Region.HasValue)
            {
                cityDto.RegionName = GetEnumDescription(city.Region.Value);
            }


            _logger.LogInformation("Successfully retrieved city details for ID: {CityId}", request.CityId);
            return GetCityByIdResponse.Success(cityDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting city details for ID: {CityId}", request.CityId);
            return GetCityByIdResponse.Failed("An error occurred while retrieving city details");
        }
    }
    private static string GetEnumDescription(Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attribute = (DescriptionAttribute?)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
        return attribute == null ? value.ToString() : attribute.Description;
    }
}