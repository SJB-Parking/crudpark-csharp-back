using CrudPark_Back.Data;
using CrudPark_Back.Repositories.Interfaces;
using CrudPark_Back.Repositories.Implementations;
using CrudPark_Back.Services.Interfaces;
using CrudPark_Back.Services.Implementations;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// ⭐ IMPORTANTE: Configurar para escuchar en todas las interfaces de red
builder.WebHost.UseUrls("http://0.0.0.0:5229", "https://0.0.0.0:7229");

// ⭐ Configure CORS - Permitir todas las IPs de la red local
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()  // Permite cualquier origen (para desarrollo local)
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

// Registrar Repositories
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddScoped<IOperatorRepository, OperatorRepository>();
builder.Services.AddScoped<IRateRepository, RateRepository>();

// Registrar Services
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();
builder.Services.AddScoped<IOperatorService, OperatorService>();
builder.Services.AddScoped<IRateService, RateService>();
builder.Services.AddScoped<IEmailService, EmailService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ⭐ IMPORTANTE: Comentar esta línea para permitir HTTP en red local
// app.UseHttpsRedirection();

// ⭐ IMPORTANTE: UseCors debe ir ANTES de UseAuthorization
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();