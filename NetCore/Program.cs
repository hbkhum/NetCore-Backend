using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NetCore.Infrastructure.Context;
using NetCore.Infrastructure.Interfaces;
using NetCore.Repositories;
using NetCore.Services.Interfaces;
using NetCore.Services;
using NetCore.Hub;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Agrega y configura servicios
builder.Services.AddDbContext<VehicleContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 21)),
        mySqlOptions => mySqlOptions.EnableRetryOnFailure());


});

builder.Services.TryAddTransient<ICarRepository, CarRepository>();
builder.Services.TryAddTransient<ITruckRepository, TruckRepository>();

builder.Services.TryAddTransient<ICarService, CarService>();
builder.Services.TryAddTransient<ITruckService, TruckService>();

builder.Services.TryAddTransient<IDataServices, DataServices>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.TryAddTransient<IDataRepositories, DataRepositories>();

builder.Services.AddSignalR();

builder.Services.AddControllers().AddNewtonsoftJson(options => {
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder => builder
        .WithOrigins("http://localhost:4200")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials()
    );
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
        options.Stores.MaxLengthForKeys = 128)
    .AddEntityFrameworkStores<VehicleContext>()
    .AddDefaultTokenProviders();

var app = builder.Build();

// Swagger en ambiente de desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Redirección HTTPS
app.UseHttpsRedirection();

// Middleware de enrutamiento necesario antes de definir los endpoints
app.UseRouting();

// Usa los servicios CORS - asegúrate que este está entre UseRouting y UseAuthorization
app.UseCors("CorsPolicy");

// Middleware de autorización
app.UseAuthorization();

// Definición de los endpoints
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers(); // Mapea los controladores de la API
    endpoints.MapHub<CarHub>("/hubs/car"); // Mapea el hub de SignalR para Car
    endpoints.MapHub<TruckHub>("/hubs/truck"); // Mapea el hub de SignalR para Truck
});

// Aplica las migraciones de base de datos automáticamente al iniciar la aplicación
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<VehicleContext>();
    context.Database.Migrate();
}

// Inicia la aplicación
app.Run();
