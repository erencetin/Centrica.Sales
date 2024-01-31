using Centrica.Sales.Data.Interfaces;
using Centrica.Sales.Data.Repository;
using Centrica.Sales.Services.Interfaces;
using Centrica.Sales.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add services to the container.
builder.Services.AddScoped<ISalespersonService, SalespersonService>();
builder.Services.AddScoped<IDistrictService, DistrictService>();
builder.Services.AddScoped<ISalespersonDistrictService, SalespersonDistrictService>();
builder.Services.AddScoped<IStoreService, StoreService>();

// Pass the connection string to the repository constructors
builder.Services.AddScoped<ISalespersonRepository>(provider => new SalespersonRepository(connectionString));
builder.Services.AddScoped<IDistrictRepository>(provider => new DistrictRepository(connectionString));
builder.Services.AddScoped<ISalespersonDistrictRepository>(provider => new SalespersonDistrictRepository(connectionString));
builder.Services.AddScoped<IStoreRepository>(provider => new StoreRepository(connectionString));
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});
builder.Services.AddControllers();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseExceptionHandler("/error");
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCors("AllowAll");

app.MapControllers();

app.Run();
