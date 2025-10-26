using MediatR;
using Microsoft.Extensions.Logging;
using AutoMapper;

using AppBookingTour.Domain.Entities;
using AppBookingTour.Application.IRepositories;
using AppBookingTour.Application.Features.Combos.GetComboById;

namespace AppBookingTour.Application.Features.Combos.CreateCombo;

public sealed class CreateComboCommandHandler : IRequestHandler<CreateComboCommand, CreateComboResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateComboCommandHandler> _logger;
    private readonly IMapper _mapper;

    public CreateComboCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<CreateComboCommandHandler> logger,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<CreateComboResponse> Handle(CreateComboCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating a new combo");
        try
        {
            var combo = _mapper.Map<Combo>(request.ComboRequest);

            combo.Rating = 0;
            combo.TotalBookings = 0;
            combo.ViewCount = 0;
            combo.InterestCount = 0;
            combo.CreatedAt = DateTime.UtcNow;

            foreach (var schedule in combo.Schedules)
            {
                schedule.BookedSlots = 0;
            }

            await _unitOfWork.Repository<Combo>().AddAsync(combo, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var comboDto = _mapper.Map<ComboDTO>(combo);
            comboDto.Schedules = comboDto.Schedules.OrderBy(s => s.DepartureDate).ToList();

            return CreateComboResponse.Success(comboDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating combo");
            return CreateComboResponse.Failed("Create combo failed: " + ex.Message);
        }
    }
}