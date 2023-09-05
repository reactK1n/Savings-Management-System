using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using SavingsManagementSystem.Data.AddToRole;
using SavingsManagementSystem.Extensions;
using SavingsManagementSystem.Policies;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
	.AddFluentValidation(fv =>
	fv.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));

//register dbContext
builder.Services.AddDbContextAndConfigurations(builder.Configuration);
//adding dependency injection container
builder.Services.AddDependencyInjection();
//Configure Identity options
builder.Services.AddIdentityConfig();
//Configure Authentication
builder.Services.AddAuthenticationConfig(builder.Configuration);
//adding policy authourization
builder.Services.AddPolicyAuthorization();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//swagger configuration for authorization
builder.Services.AddSwaggerConfig();


builder.Services.AddCors(c =>
{
	c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin());
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.InitRoles();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
