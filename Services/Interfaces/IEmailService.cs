using CrudPark_Back.Models.Entities;

namespace CrudPark_Back.Services.Interfaces;

public interface IEmailService
{
    Task SendSubscriptionCreatedEmailAsync(string toEmail, string customerName, MonthlySubscription subscription);
    Task SendSubscriptionExpiringEmailAsync(string toEmail, string customerName, MonthlySubscription subscription);
    Task SendTestEmailAsync(string toEmail);
}