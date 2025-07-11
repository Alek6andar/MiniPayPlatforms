using MiniPay.Api.Services;
using System.Text.Json;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Registracija na servisite
builder.Services.AddScoped<IPaymentProviderRepository, JsonPaymentProviderRepository>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddHttpClient();

// Dodavanje na controllers i postavki za JSON serialization so camelCase
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase; // <-- camelCase
        options.JsonSerializerOptions.WriteIndented = true;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

// Swagger konfiguracija
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "MiniPay API",
        Version = "v1",
        Description = "Payment provider management system",
        Contact = new OpenApiContact
        {
            Name = "Aco Krstesi",
            Email = "aco@example.com"
        }
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

// CORS policy za Angular development
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDev",
        policy => policy.WithOrigins("http://localhost:4200")
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .AllowCredentials());
});

var app = builder.Build();

var dataDir = Path.Combine(Directory.GetCurrentDirectory(), "Data");
if (!Directory.Exists(dataDir))
{
    Directory.CreateDirectory(dataDir);
}

// Middleware konfiguracija za dev environment
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MiniPay API v1");
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowAngularDev");
app.UseAuthorization();

app.MapControllers();

app.Run();
