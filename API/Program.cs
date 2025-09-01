using API.Middlewares;
using Application.CQRS.Results;
using Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Persistence;
using Persistence.Seed;
using Persistence.ServiceRegistration;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System.Collections.ObjectModel;
using System.Data;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

//var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// ===================== Serilog Configuration =====================
var sinkOpts = new MSSqlServerSinkOptions
{
    TableName = "Logs",
    AutoCreateSqlTable = true
};

var columnOpts = new ColumnOptions();
columnOpts.AdditionalColumns = new Collection<SqlColumn>
{
    new SqlColumn("CorrelationId", SqlDbType.NVarChar, dataLength: 64),
    new SqlColumn("UserName", SqlDbType.NVarChar, dataLength: 128)
};

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .Enrich.WithProperty("Application", "Todora")
    .WriteTo.Console()
    .WriteTo.MSSqlServer(
        connectionString: connectionString,
        sinkOptions: sinkOpts,
        columnOptions: columnOpts
    )
    .CreateLogger();

builder.Host.UseSerilog();

// ===================== Services =====================
builder.Services.AddPersistenceServices();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


// ===================== Identity Configuration =====================
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    // Password Settings
    options.Password.RequireDigit = true; 
    options.Password.RequiredLength = 8; 
    options.Password.RequireLowercase = true; 
    options.Password.RequireUppercase = true; 
    options.Password.RequireNonAlphanumeric = true; 

    // User Settings
    options.User.RequireUniqueEmail = true;

    // Lockout Settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders(); 



// ===================== JWT Authentication =====================
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
        )
    };

    // ===================== Standard JSON for Unauthorized/Forbidden =====================
    options.Events = new JwtBearerEvents
    {
        OnChallenge = context =>
        {
            context.HandleResponse(); // default response engelle
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 401;

            var result = Result<object>.Unauthorized();
            var json = JsonSerializer.Serialize(result, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            return context.Response.WriteAsync(json);
        },
        OnForbidden = context =>
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 403;

            var result = Result<object>.Forbidden();
            var json = JsonSerializer.Serialize(result, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            return context.Response.WriteAsync(json);
        }
    };

});

// ===================== CORS =====================

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy(name: MyAllowSpecificOrigins, policy =>
//    {
//        // Blazor uygulamanýn çalýþacaðý adresi buraya yaz
//        policy.WithOrigins("https://localhost:5001", "http://localhost:5000")
//              .AllowAnyHeader()
//              .AllowAnyMethod();
//    });
//});

// ===================== Swagger =====================
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT için 'Bearer {token}' formatýný kullanýn"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

var app = builder.Build();

// ===================== Middleware Pipeline =====================

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todora API V1");
        c.RoutePrefix = string.Empty;
    });
}

// Invoking Seeder

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    try
    {
        await DataSeeder.SeedAsync(serviceProvider);
        Log.Information("Veritabaný baþarýyla seed'lendi.");
    }
    catch (Exception ex)
    {
        Log.Error(ex, "Veritabaný seed'lenirken bir hata oluþtu.");
    }
}


app.UseHttpsRedirection();

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
app.UseMiddleware<ResultWrapperMiddleware>();
app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<LoggingMiddleware>();
//app.UseCors();

// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// Controllers
app.MapControllers();

app.Run();


