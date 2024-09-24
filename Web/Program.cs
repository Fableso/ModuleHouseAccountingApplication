using Application;
using Infrastructure;
using Infrastructure.Identity;
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
app.UseRouting();

await DbInitializer.SeedRolesAsync(app.Services);
if (app.Environment.IsDevelopment())
{
    await DbInitializer.SeedAccountsAsync(app.Services);
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();