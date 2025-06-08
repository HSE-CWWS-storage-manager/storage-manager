using System.Reflection;
using backend;
using backend.Auth;
using backend.Filters;
using backend.Mapping;
using backend.Mapping.Impl;
using backend.Services;
using backend.Services.Impl;
using backend.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using static backend.Utils.StringConstants;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("StorageManagerConnection");
builder.Services.AddDbContext<StorageManagerDbContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<StorageManagerDbContext>()
    .AddDefaultTokenProviders();

var storageManagerOrigins = "_storageManagerOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: storageManagerOrigins,
        policy =>
        {
            policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        });
});

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = AuthOptions.Issuer,

            ValidateAudience = true,
            ValidAudience = AuthOptions.Audience,
            ValidateLifetime = true,

            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
            ValidateIssuerSigningKey = true
        };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"Authentication failed: {context.Exception}");
                return Task.CompletedTask;
            },
            OnTokenValidated = _ =>
            {
                Console.WriteLine("Token validated successfully");
                return Task.CompletedTask;
            }
        };
    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(
        EditorPolicy,
        policy => policy.RequireRole(OperatorRole, AdminRole)
    );
});

builder.Services.AddTransient<IUserMapper, UserMapper>();
builder.Services.AddTransient<IAccountService, AccountService>();
builder.Services.AddTransient<IEquipmentService, EquipmentService>();
builder.Services.AddTransient<IWarehouseService, WarehouseService>();
builder.Services.AddTransient<IStudentService, StudentService>();
builder.Services.AddTransient<IEquipmentOperationService, EquipmentOperationService>();
builder.Services.AddTransient<IEquipmentCardService, EquipmentCardService>();

builder.Services.AddControllers(options => options.Filters.Add<HttpResponseExceptionFilter>());
builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Storage Manager",
        Version = "v1",
        Description = "ИС для учета ТМЦ ЦРС НИУ ВШЭ - Пермь"
    });
    
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
});

builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    if (!await roleManager.RoleExistsAsync(AdminRole))
        await roleManager.CreateAsync(new IdentityRole(AdminRole));

    if (!await roleManager.RoleExistsAsync(OperatorRole))
        await roleManager.CreateAsync(new IdentityRole(OperatorRole));

    if (!await roleManager.RoleExistsAsync(UserRole))
        await roleManager.CreateAsync(new IdentityRole(UserRole));
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty;
    });
}
else
{
    app.UseExceptionHandler("/error");
}

app.UseHttpsRedirection();

app.UseCors(storageManagerOrigins);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();