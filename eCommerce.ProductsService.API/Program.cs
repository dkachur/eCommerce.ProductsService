using eCommerce.ProductsService.API.Exntensions;
using eCommerce.ProductsService.API.Middlewares;
using eCommerce.ProductsService.Application;
using eCommerce.ProductsService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add Application and infrastructure serivces
builder.Services.AddInfrastructure(builder.Configuration, builder.Environment)
                .AddApplication();

// Add FluentValidation validatiors to services
builder.Services.AddFluentValidation();

// Add Authorization and Authentication
builder.Services.AddAuthorization()
                .AddAuthentication();

// Add Enum converter
builder.Services.AddJsonStringEnumConverter();

// Configure Swagger
if (!builder.Environment.IsEnvironment("Testing"))
    builder.Services.AddSwaggerConfig();

// Configure CORS
builder.Services.AddConfiguredCors();


var app = builder.Build();

app.UseExceptionHandlingMiddleware();
app.UseRouting();

app.UseCors();

app.UseHttpsRedirection();

if (!builder.Environment.IsEnvironment("Testing"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
    
app.UseAuthentication();
app.UseAuthorization();

app.MapProductEndpoints();

app.Run();

public partial class Program { }