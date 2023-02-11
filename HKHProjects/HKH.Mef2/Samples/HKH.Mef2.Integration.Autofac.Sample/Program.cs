using System.Composition.Hosting;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using HKH.Mef2.Integration.Autofac;
using Sample.Service;
using Sample.Service.MefServices;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//initlize auto container
builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
{
    builder.RegisterType<SingleService>().AsImplementedInterfaces().SingleInstance();
    builder.RegisterType<ScopedService>().AsImplementedInterfaces().InstancePerLifetimeScope();

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
