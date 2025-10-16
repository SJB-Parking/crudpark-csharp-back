using CrudPark_Back.Models.Entities;
using CrudPark_Back.Services.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace CrudPark_Back.Services.Implementations;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendSubscriptionCreatedEmailAsync(string toEmail, string customerName, MonthlySubscription subscription)
    {
        var subject = "¡Mensualidad Creada Exitosamente! - CrudPark";
        var body = $@"
            <h2>Hola {customerName},</h2>
            <p>Tu mensualidad ha sido creada exitosamente.</p>
            <h3>Detalles de la Mensualidad:</h3>
            <ul>
                <li><strong>Código:</strong> {subscription.SubscriptionCode}</li>
                <li><strong>Fecha de Inicio:</strong> {subscription.StartDate:dd/MM/yyyy}</li>
                <li><strong>Fecha de Fin:</strong> {subscription.EndDate:dd/MM/yyyy}</li>
                <li><strong>Monto Pagado:</strong> ${subscription.AmountPaid:N0} COP</li>
                <li><strong>Vehículos Permitidos:</strong> {subscription.MaxVehicles}</li>
            </ul>
            <p>Gracias por confiar en CrudPark.</p>
            <p><em>Este es un correo automático, por favor no responder.</em></p>
        ";

        await SendEmailAsync(toEmail, subject, body);
    }

    public async Task SendSubscriptionExpiringEmailAsync(string toEmail, string customerName, MonthlySubscription subscription)
    {
        var daysRemaining = (subscription.EndDate.Date - DateTime.UtcNow.Date).Days;
        
        var subject = "⚠️ Tu Mensualidad está por Vencer - CrudPark";
        var body = $@"
            <h2>Hola {customerName},</h2>
            <p>Te recordamos que tu mensualidad está próxima a vencer.</p>
            <h3>Detalles:</h3>
            <ul>
                <li><strong>Código:</strong> {subscription.SubscriptionCode}</li>
                <li><strong>Fecha de Vencimiento:</strong> {subscription.EndDate:dd/MM/yyyy}</li>
                <li><strong>Días Restantes:</strong> {daysRemaining} día(s)</li>
            </ul>
            <p>Por favor, renueva tu mensualidad para evitar interrupciones en el servicio.</p>
            <p>Gracias por confiar en CrudPark.</p>
            <p><em>Este es un correo automático, por favor no responder.</em></p>
        ";

        await SendEmailAsync(toEmail, subject, body);
    }

    public async Task SendTestEmailAsync(string toEmail)
    {
        var subject = "Test Email - CrudPark";
        var body = "<h2>Este es un correo de prueba</h2><p>Si recibiste este correo, el servicio de email está funcionando correctamente.</p>";
        
        await SendEmailAsync(toEmail, subject, body);
    }

    private async Task SendEmailAsync(string toEmail, string subject, string htmlBody)
    {
        try
        {
            var message = new MimeMessage();
            
            // Configuración del remitente (obtener de appsettings.json)
            var fromEmail = _configuration["EmailSettings:FromEmail"] ?? "noreply@crudpark.com";
            var fromName = _configuration["EmailSettings:FromName"] ?? "CrudPark";
            
            message.From.Add(new MailboxAddress(fromName, fromEmail));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = htmlBody
            };
            message.Body = bodyBuilder.ToMessageBody();

            // Configuración SMTP (obtener de appsettings.json)
            var smtpHost = _configuration["EmailSettings:SmtpHost"] ?? "smtp.gmail.com";
            var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"] ?? "587");
            var smtpUsername = _configuration["EmailSettings:SmtpUsername"] ?? "";
            var smtpPassword = _configuration["EmailSettings:SmtpPassword"] ?? "";

            using var client = new SmtpClient();
            
            await client.ConnectAsync(smtpHost, smtpPort, SecureSocketOptions.StartTls);
            
            if (!string.IsNullOrEmpty(smtpUsername) && !string.IsNullOrEmpty(smtpPassword))
            {
                await client.AuthenticateAsync(smtpUsername, smtpPassword);
            }
            
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
        catch (Exception ex)
        {
            // Log the error (puedes usar ILogger aquí)
            throw new InvalidOperationException($"Error al enviar email: {ex.Message}", ex);
        }
    }
}