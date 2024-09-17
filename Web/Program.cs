using System.Reflection;
using Application;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure;
using Web.Extensions;
using Web.Middleware;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers()
    .AddStronglyTypedIdConversionJsonOptions()
    .AddCustomModelBinders();

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseRouting();
app.MapControllers();

await app.RunAsync();