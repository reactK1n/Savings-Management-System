using SavingsManagementSystem.Extensions;
using SavingsManagementSystem.Policies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
//register dbContext
builder.Services.AddDbContextAndConfigurations(builder.Configuration);
//Configure Identity options
builder.Services.AddIdentityConfig();
//Configure Authentication
builder.Services.AddAuthenticationConfig(builder.Configuration);
//adding policy authourization
builder.Services.AddPolicyAuthorization();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerConfig();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
