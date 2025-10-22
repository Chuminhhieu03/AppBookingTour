using AppBookingTour.Application.Features.Cities.GetCityById;
using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System.ComponentModel;

namespace AppBookingTour.Application.Features.Cities.GetListCity;

public sealed class GetListCityQueryHandler : IRequestHandler<GetListCityQuery, GetListCityResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetListCityQueryHandler> _logger;
    private readonly IMapper _mapper;

    public GetListCityQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetListCityQueryHandler> logger,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<GetListCityResponse> Handle(GetListCityQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting all cities");
        try
        {
            var cities = await _unitOfWork.Repository<City>().GetAllAsync(cancellationToken);

            var cityDtos = _mapper.Map<List<CityDTO>>(cities);

            cityDtos = cityDtos.OrderBy(c => c.Id).ToList();

            return GetListCityResponse.Success(cityDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting all cities");
            return GetListCityResponse.Failed("An error occurred while retrieving cities.");
        }
    }
}