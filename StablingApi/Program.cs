using Microsoft.EntityFrameworkCore;
using StablingApi.Contexts;
using StablingApi.Repositories;
using StablingApi.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocument(options =>
{
    options.Description = "API ИС поддержки управления КСК";
    options.Version = "v1";
    options.Title = "StablingApi";
});

string connectionString = "Server=tcp:127.0.0.1,45565;Database=TimeTable;User Id=StablingUser;Password=KSKDedovsk2022;Trusted_Connection=True;TrustServerCertificate=True;";

builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<ITrainingRepository, TrainingRepository>();
builder.Services.AddScoped<ITrainingTypeRepository, TrainingTypeRepository>();
builder.Services.AddScoped<IAbonementMarkRepository, AbonementMarkRepository>();
builder.Services.AddScoped<IAbonementRepository, AbonementRepository>();
builder.Services.AddScoped<IAbonementTypeRepository, AbonementTypeRepository>();
builder.Services.AddScoped<ITrainerRepository, TrainerRepository>();

builder.Services.AddDbContextFactory<ClientContext>(o => o.UseSqlServer(connectionString));
builder.Services.AddDbContextFactory<TrainingsContext>(o => o.UseSqlServer(connectionString));
builder.Services.AddDbContextFactory<AbonementContext>(o => o.UseSqlServer(connectionString));
builder.Services.AddDbContextFactory<TrainerContext>(o => o.UseSqlServer(connectionString));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
