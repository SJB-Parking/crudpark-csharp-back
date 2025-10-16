using CrudPark_Back.Models.DTOs.Requests;
using CrudPark_Back.Models.DTOs.Responses;
using CrudPark_Back.Models.Entities;
using CrudPark_Back.Repositories.Interfaces;
using CrudPark_Back.Services.Interfaces;

namespace CrudPark_Back.Services.Implementations;

public class SubscriptionService : ISubscriptionService
{
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IEmailService _emailService;

    public SubscriptionService(
        ISubscriptionRepository subscriptionRepository,
        ICustomerRepository customerRepository,
        IVehicleRepository vehicleRepository,
        IEmailService emailService)
    {
        _subscriptionRepository = subscriptionRepository;
        _customerRepository = customerRepository;
        _vehicleRepository = vehicleRepository;
        _emailService = emailService;
    }

    public async Task<IEnumerable<SubscriptionResponse>> GetAllSubscriptionsAsync()
    {
        var subscriptions = await _subscriptionRepository.GetAllAsync();
        return subscriptions.Select(MapToResponse);
    }

    public async Task<SubscriptionResponse?> GetSubscriptionByIdAsync(int id)
    {
        var subscription = await _subscriptionRepository.GetByIdAsync(id);
        return subscription == null ? null : MapToResponse(subscription);
    }

    public async Task<IEnumerable<SubscriptionResponse>> GetActiveSubscriptionsAsync()
    {
        var subscriptions = await _subscriptionRepository.GetActiveSubscriptionsAsync();
        return subscriptions.Select(MapToResponse);
    }

    public async Task<IEnumerable<SubscriptionResponse>> GetExpiringSoonAsync(int daysThreshold = 3)
    {
        var subscriptions = await _subscriptionRepository.GetExpiringSoonAsync(daysThreshold);
        return subscriptions.Select(MapToResponse);
    }

    public async Task<IEnumerable<SubscriptionResponse>> GetExpiredSubscriptionsAsync()
    {
        var subscriptions = await _subscriptionRepository.GetExpiredSubscriptionsAsync();
        return subscriptions.Select(MapToResponse);
    }

    public async Task<SubscriptionResponse> CreateSubscriptionAsync(CreateSubscriptionRequest request)
    {
        // Validar que el cliente existe
        var customer = await _customerRepository.GetByIdAsync(request.CustomerId);
        if (customer == null)
            throw new KeyNotFoundException("Cliente no encontrado");

        // Validar fechas
        if (request.EndDate <= request.StartDate)
            throw new InvalidOperationException("La fecha de fin debe ser posterior a la fecha de inicio");

        // Validar vehículos
        if (request.VehicleIds.Count > request.MaxVehicles)
            throw new InvalidOperationException($"No puede agregar más de {request.MaxVehicles} vehículos");

        // Generar código único
        var subscriptionCode = GenerateSubscriptionCode();

        // Crear mensualidad
        var subscription = new MonthlySubscription
        {
            CustomerId = request.CustomerId,
            SubscriptionCode = subscriptionCode,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            AmountPaid = request.AmountPaid,
            MaxVehicles = request.MaxVehicles,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        subscription = await _subscriptionRepository.CreateAsync(subscription);

        // Agregar vehículos
        foreach (var vehicleId in request.VehicleIds)
        {
            await _subscriptionRepository.AddVehicleToSubscriptionAsync(subscription.Id, vehicleId);
        }

        // Enviar email de confirmación
        try
        {
            await _emailService.SendSubscriptionCreatedEmailAsync(customer.Email, customer.FullName, subscription);
        }
        catch (Exception)
        {
            // Log el error pero no fallar la creación
        }

        // Recargar con relaciones
        var createdSubscription = await _subscriptionRepository.GetByIdAsync(subscription.Id);
        return MapToResponse(createdSubscription!);
    }

    public async Task<SubscriptionResponse> UpdateSubscriptionAsync(int id, UpdateSubscriptionRequest request)
    {
        var subscription = await _subscriptionRepository.GetByIdAsync(id);
        if (subscription == null)
            throw new KeyNotFoundException("Mensualidad no encontrada");

        // Actualizar campos
        if (request.StartDate.HasValue)
            subscription.StartDate = request.StartDate.Value;
        
        if (request.EndDate.HasValue)
        {
            if (request.EndDate.Value <= subscription.StartDate)
                throw new InvalidOperationException("La fecha de fin debe ser posterior a la fecha de inicio");
            subscription.EndDate = request.EndDate.Value;
        }
        
        if (request.AmountPaid.HasValue)
            subscription.AmountPaid = request.AmountPaid.Value;
        
        if (request.IsActive.HasValue)
            subscription.IsActive = request.IsActive.Value;

        subscription = await _subscriptionRepository.UpdateAsync(subscription);

        // Recargar con relaciones
        var updatedSubscription = await _subscriptionRepository.GetByIdAsync(subscription.Id);
        return MapToResponse(updatedSubscription!);
    }

    public async Task<bool> DeleteSubscriptionAsync(int id)
    {
        return await _subscriptionRepository.DeleteAsync(id);
    }

    public async Task<bool> AddVehicleToSubscriptionAsync(int subscriptionId, AddVehicleToSubscriptionRequest request)
    {
        // Validar que el vehículo existe
        var vehicle = await _vehicleRepository.GetByIdAsync(request.VehicleId);
        if (vehicle == null)
            throw new KeyNotFoundException("Vehículo no encontrado");

        return await _subscriptionRepository.AddVehicleToSubscriptionAsync(subscriptionId, request.VehicleId);
    }

    public async Task<bool> RemoveVehicleFromSubscriptionAsync(int subscriptionId, int vehicleId)
    {
        return await _subscriptionRepository.RemoveVehicleFromSubscriptionAsync(subscriptionId, vehicleId);
    }

    public async Task CheckAndNotifyExpiringSubscriptionsAsync()
    {
        var expiringSubscriptions = await _subscriptionRepository.GetExpiringSoonAsync(3);
        
        foreach (var subscription in expiringSubscriptions)
        {
            try
            {
                await _emailService.SendSubscriptionExpiringEmailAsync(
                    subscription.Customer.Email,
                    subscription.Customer.FullName,
                    subscription);
            }
            catch (Exception)
            {
                // Log el error pero continuar con las demás
            }
        }
    }

    private string GenerateSubscriptionCode()
    {
        var year = DateTime.UtcNow.Year;
        var random = new Random().Next(1000, 9999);
        return $"SUB-{year}-{random}";
    }

    private SubscriptionResponse MapToResponse(MonthlySubscription subscription)
    {
        var daysRemaining = (subscription.EndDate.Date - DateTime.UtcNow.Date).Days;
        
        return new SubscriptionResponse
        {
            Id = subscription.Id,
            SubscriptionCode = subscription.SubscriptionCode,
            Customer = new CustomerResponse
            {
                Id = subscription.Customer.Id,
                FullName = subscription.Customer.FullName,
                Email = subscription.Customer.Email,
                Phone = subscription.Customer.Phone,
                IdentificationNumber = subscription.Customer.IdentificationNumber
            },
            StartDate = subscription.StartDate,
            EndDate = subscription.EndDate,
            IsActive = subscription.IsActive,
            AmountPaid = subscription.AmountPaid,
            MaxVehicles = subscription.MaxVehicles,
            Vehicles = subscription.SubscriptionVehicles?.Select(sv => new VehicleResponse
            {
                Id = sv.Vehicle.Id,
                LicensePlate = sv.Vehicle.LicensePlate,
                VehicleType = sv.Vehicle.VehicleType.ToString(),
                Brand = sv.Vehicle.Brand,
                Model = sv.Vehicle.Model,
                Color = sv.Vehicle.Color
            }).ToList() ?? new List<VehicleResponse>(),
            DaysRemaining = daysRemaining
        };
    }
}