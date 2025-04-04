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
using System.Reflection;
using System.Text;
using final_project_backend.Handlers.Message;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

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

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEntityFrameworkSqlServer();
builder.Services.AddSignalR();
builder.Services.AddDbContextPool<FinalProjectTrainingDbContext>(options =>
{
    var conString = configuration.GetConnectionString("SQLServerDB");
    options.UseSqlServer(conString);
});
builder.Services.AddControllers();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Misc
builder.Services.AddDataProtection();
builder.Services.AddHttpContextAccessor();

// Validators
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

// MediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(
        typeof(CreateMessageHandler).Assembly
    );
});

// Services
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<ItemService>();
builder.Services.AddTransient<UserService>();
builder.Services.AddTransient<ProductImageService>();
builder.Services.AddTransient<ShopService>();
builder.Services.AddTransient<CartService>();
builder.Services.AddSingleton<BlobStorageService>();
builder.Services.AddSingleton<JwtService>();
builder.Services.AddHostedService<OrderDetailUpdaterService>();

// Tambahan: Services untuk sistem Chat
builder.Services.AddScoped<ChatService>();
builder.Services.AddScoped<MessageService>();
builder.Services.AddScoped<ChatUserService>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Kestrel
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5252); // Allows connections from any IP
});

var app = builder.Build();

app.UseCors("AllowAll");
app.UseCors(policy =>
    policy.WithOrigins("http://localhost:8081") // Sesuaikan dengan port Next.js
          .AllowAnyMethod()
          .AllowAnyHeader()
);

// Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseAuthorization();

app.MapControllers();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<ChatHub>("/chatHub");
});

app.Run();