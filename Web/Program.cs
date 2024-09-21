using Application;
using Infrastructure;
using Web.Extensions;
using Web.Middleware;
using Web.Validation;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers()
    .AddJsonSerializationOptions()
    .AddCustomModelBinders();

builder.Services.AddValidation();
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