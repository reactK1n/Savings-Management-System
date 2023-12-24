using FluentValidation.AspNetCore;
using SavingsManagementSystem.Data.AddToRole;
using SavingsManagementSystem.Extensions;
using SavingsManagementSystem.Policies;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//adding serilog
builder.AddLogger();

builder.Services.AddControllers();

builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();

//register dbContext
builder.Services.AddDbContextAndConfigurations(builder.Environment, builder.Configuration);
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


/*builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowSpecificOrigins",
		builder =>
		{
			builder.WithOrigins("https://example.com", "https://another-domain.com")
				.AllowAnyHeader()
				.AllowAnyMethod();
		});
});*/


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

app.InitRoles();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
