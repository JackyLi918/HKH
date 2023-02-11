using System.Composition.Hosting;
using HKH.Mef2.Integration;
using Sample.Service;
using Sample.Service.MefServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//initlize default container
builder.Host.ConfigureContainer<IServiceCollection>(builder =>
{
    builder.AddSingleton<ISingleService, SingleService>();
    builder.AddScoped<IScopedService, ScopedService>();

    var mefContainer = new ContainerConfiguration();
    mefContainer.WithAssembly(typeof(IMefService).Assembly);

    builder.EnableMef2(mefContainer);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
