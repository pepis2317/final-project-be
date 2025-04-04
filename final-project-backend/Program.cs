using Entities;
using final_project_backend.Models.Cart;
using final_project_backend.Models.Item;
using final_project_backend.Models.Order;
using final_project_backend.Models.Shop;
using final_project_backend.Models.Users;
using final_project_backend.Services;
using final_project_backend.Validators.Cart;
using final_project_backend.Validators.Item;
using final_project_backend.Validators.Order;
using final_project_backend.Validators.Shop;
using final_project_backend.Validators.User;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Services;
using System.Text;
using final_project_backend.Hubs;
using final_project_backend.Handlers.Message;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Konfigurasi JWT
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "http://localhost",
            ValidAudience = "http://localhost",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("linggangguliguliguligwacalingganggu"))
        };
    });

// Tambahkan SignalR
builder.Services.AddSignalR();

// Konfigurasi Database
builder.Services.AddDbContextPool<FinalProjectTrainingDbContext>(options =>
{
    var conString = configuration.GetConnectionString("SQLServerDB");
    options.UseSqlServer(conString);
});

// Konfigurasi JSON untuk Controllers
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });

// Tambahkan Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Miscellaneous Services
builder.Services.AddDataProtection();
builder.Services.AddHttpContextAccessor();

// Tambahkan Validators
builder.Services.AddScoped<IValidator<LoginRequest>, LoginValidator>();
builder.Services.AddScoped<IValidator<UserUpdateRequest>, UserUpdateValidator>();
builder.Services.AddScoped<IValidator<CreateShopRequest>, CreateShopValidator>();
builder.Services.AddScoped<IValidator<EditShopRequest>, EditShopValidator>();
builder.Services.AddScoped<IValidator<CreateItemRequest>, CreateItemValidator>();
builder.Services.AddScoped<IValidator<EditItemRequest>, EditItemValidator>();
builder.Services.AddScoped<IValidator<RegisterRequest>, RegisterValidator>();
builder.Services.AddScoped<IValidator<CreateOrderRequest>, CreateOrderValidator>();
builder.Services.AddScoped<IValidator<UpdateOrderRequest>, UpdateOrderValidator>();
builder.Services.AddScoped<IValidator<ItemQuery>, ItemQueryValidator>();
builder.Services.AddScoped<IValidator<CartItemRequest>, PostIncompleteCartValidator>();
builder.Services.AddScoped<IValidator<CartItemEditRequest>, EditIncompleteCartValidator>();

// Tambahkan MediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(typeof(CreateMessageHandler).Assembly);
});

// Tambahkan Services
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<ItemService>();
builder.Services.AddTransient<UserService>();
builder.Services.AddTransient<ProductImageService>();
builder.Services.AddTransient<ShopService>();
builder.Services.AddTransient<CartService>();
builder.Services.AddSingleton<BlobStorageService>();
builder.Services.AddSingleton<JwtService>();
builder.Services.AddHostedService<OrderDetailUpdaterService>();

// Tambahkan Services untuk Chat System
builder.Services.AddScoped<ChatService>();
builder.Services.AddScoped<MessageService>();
builder.Services.AddScoped<ChatUserService>();

// Konfigurasi CORS (Hanya gunakan satu konfigurasi yang benar)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://localhost:3000", "http://localhost:8081", "http://192.168.1.3:3000", "http://192.168.18.64:3000") // Sesuaikan dengan frontend
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowAnyMethod().AllowCredentials(); ;
    });
});

// **Bangun Aplikasi**
var app = builder.Build();

// **Gunakan Middleware yang Benar**
app.UseCors("AllowAll");

// Gunakan Swagger hanya saat development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware lainnya
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<ChatHub>("/chatHub");


app.Run();