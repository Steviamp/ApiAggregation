using ApiAggregation.Application.Interfaces;
using ApiAggregation.Application.Queries.AggregatedData;
using ApiAggregation.Application.Services;
using ApiAggregation.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Infrastructure
builder.Services.AddInfrastructure(builder.Configuration);

// Add Application Services
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAggregatedDataQuery).Assembly));
builder.Services.AddScoped<IAggregationService, AggregationService>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
