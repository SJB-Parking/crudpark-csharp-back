using CrudPark_Back.Models.DTOs.Requests;
using CrudPark_Back.Models.DTOs.Responses;
using CrudPark_Back.Models.Entities;
using CrudPark_Back.Repositories.Interfaces;
using CrudPark_Back.Services.Interfaces;

namespace CrudPark_Back.Services.Implementations;

public class RateService : IRateService
{
    private readonly IRateRepository _rateRepository;

    public RateService(IRateRepository rateRepository)
    {
        _rateRepository = rateRepository;
    }

    public async Task<IEnumerable<RateResponse>> GetAllRatesAsync()
    {
        var rates = await _rateRepository.GetAllAsync();
        return rates.Select(MapToResponse);
    }

    public async Task<RateResponse?> GetRateByIdAsync(int id)
    {
        var rate = await _rateRepository.GetByIdAsync(id);
        return rate == null ? null : MapToResponse(rate);
    }

    public async Task<RateResponse?> GetActiveRateAsync()
    {
        var rate = await _rateRepository.GetActiveRateAsync();
        return rate == null ? null : MapToResponse(rate);
    }

    public async Task<RateResponse> CreateRateAsync(CreateRateRequest request)
    {
        // Si se marca como activa, desactivar todas las demás
        if (request.EffectiveFrom <= DateTime.UtcNow)
        {
            await _rateRepository.DeactivateAllRatesAsync();
        }

        var rate = new Rate
        {
            RateName = request.RateName,
            HourlyRate = request.HourlyRate,
            FractionRate = request.FractionRate,
            DailyCap = request.DailyCap,
            GracePeriodMinutes = request.GracePeriodMinutes,
            IsActive = request.EffectiveFrom <= DateTime.UtcNow,
            EffectiveFrom = request.EffectiveFrom,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        rate = await _rateRepository.CreateAsync(rate);
        return MapToResponse(rate);
    }

    public async Task<RateResponse> UpdateRateAsync(int id, UpdateRateRequest request)
    {
        var rate = await _rateRepository.GetByIdAsync(id);
        if (rate == null)
            throw new KeyNotFoundException("Tarifa no encontrada");

        // Actualizar campos
        if (!string.IsNullOrEmpty(request.RateName))
            rate.RateName = request.RateName;
        
        if (request.HourlyRate.HasValue)
            rate.HourlyRate = request.HourlyRate.Value;
        
        if (request.FractionRate.HasValue)
            rate.FractionRate = request.FractionRate.Value;
        
        if (request.DailyCap.HasValue)
            rate.DailyCap = request.DailyCap.Value;
        
        if (request.GracePeriodMinutes.HasValue)
            rate.GracePeriodMinutes = request.GracePeriodMinutes.Value;
        
        // Si se activa esta tarifa, desactivar las demás
        if (request.IsActive.HasValue && request.IsActive.Value && !rate.IsActive)
        {
            await _rateRepository.DeactivateAllRatesAsync();
            rate.IsActive = true;
        }
        else if (request.IsActive.HasValue)
        {
            rate.IsActive = request.IsActive.Value;
        }

        rate = await _rateRepository.UpdateAsync(rate);
        return MapToResponse(rate);
    }

    public async Task<bool> DeleteRateAsync(int id)
    {
        return await _rateRepository.DeleteAsync(id);
    }

    public async Task<decimal> CalculateParkingFeeAsync(DateTime entryTime, DateTime exitTime)
    {
        var rate = await _rateRepository.GetActiveRateAsync();
        if (rate == null)
            throw new InvalidOperationException("No hay una tarifa activa configurada");

        // Calcular duración en minutos
        var duration = (exitTime - entryTime).TotalMinutes;

        // Aplicar tiempo de gracia
        if (duration <= rate.GracePeriodMinutes)
            return 0;

        // Calcular duración efectiva (después del tiempo de gracia)
        var effectiveDuration = duration - rate.GracePeriodMinutes;

        // Calcular horas completas y fracciones
        var hours = (int)(effectiveDuration / 60);
        var remainingMinutes = effectiveDuration % 60;

        // Calcular costo
        decimal totalCost = 0;

        // Costo por horas completas
        totalCost += hours * rate.HourlyRate;

        // Costo por fracción (si hay minutos sobrantes)
        if (remainingMinutes > 0)
            totalCost += rate.FractionRate;

        // Aplicar tope diario si existe
        if (rate.DailyCap.HasValue && totalCost > rate.DailyCap.Value)
            totalCost = rate.DailyCap.Value;

        return totalCost;
    }

    private RateResponse MapToResponse(Rate rate)
    {
        return new RateResponse
        {
            Id = rate.Id,
            RateName = rate.RateName,
            HourlyRate = rate.HourlyRate,
            FractionRate = rate.FractionRate,
            DailyCap = rate.DailyCap,
            GracePeriodMinutes = rate.GracePeriodMinutes,
            IsActive = rate.IsActive,
            EffectiveFrom = rate.EffectiveFrom
        };
    }
}