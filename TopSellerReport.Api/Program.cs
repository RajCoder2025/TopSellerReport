//using TopSellersReport.Api.

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddScoped<TopSellerReport.Api.Services.OrderService>();
//builder.Services.AddScoped<TopSellerReport.Api.Services.OrderService>();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS for frontend
builder.Services.AddCors();

var app = builder.Build();

app.UseCors(policy =>
    policy.WithOrigins("http://localhost:3000")
          .AllowAnyMethod()
          .AllowAnyHeader()
);

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
