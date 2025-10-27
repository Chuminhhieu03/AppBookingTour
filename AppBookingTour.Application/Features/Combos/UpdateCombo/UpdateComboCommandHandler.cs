using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.Combos.UpdateCombo;

public sealed class UpdateComboCommandHandler : IRequestHandler<UpdateComboCommand, UpdateComboResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateComboCommandHandler> _logger;
    private readonly IMapper _mapper;

    public UpdateComboCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<UpdateComboCommandHandler> logger,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<UpdateComboResponse> Handle(UpdateComboCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating combo with ID: {ComboId}", request.ComboId);
        try
        {
            var existingCombo = await _unitOfWork.Repository<Combo>().GetByIdAsync(request.ComboId);
            if (existingCombo == null)
            {
                return UpdateComboResponse.Failed($"Combo with ID {request.ComboId} not found.");
            }

            _mapper.Map(request.ComboRequest, existingCombo);
            existingCombo.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return UpdateComboResponse.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating combo with ID {ComboId}", request.ComboId);
            return UpdateComboResponse.Failed("An error occurred while updating the combo.");
        }
    }
}