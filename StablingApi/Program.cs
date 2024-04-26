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
builder.Services.AddScoped<IHorseRepository, HorseRepository>();
builder.Services.AddScoped<IMoneyAccountRepository, MoneyAccountRepository>();
builder.Services.AddScoped<IMoneyTransactionRepository, MoneyTransactionRepository>();
builder.Services.AddScoped<IBusinessOperationRepository,BusinessOperationRepository>();
builder.Services.AddScoped<IBusinessOperationTypeRepository, BusinessOperationTypeRepository>();

builder.Services.AddDbContextFactory<ClientsContext>(o => o.UseSqlServer(connectionString));
builder.Services.AddDbContextFactory<TrainingsContext>(o => o.UseSqlServer(connectionString));
builder.Services.AddDbContextFactory<AbonementsContext>(o => o.UseSqlServer(connectionString));
builder.Services.AddDbContextFactory<TrainersContext>(o => o.UseSqlServer(connectionString));
builder.Services.AddDbContextFactory<HorsesContext>(o => o.UseSqlServer(connectionString));
builder.Services.AddDbContextFactory<MoneyContext>(o => o.UseSqlServer(connectionString));


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
