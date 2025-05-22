using ACBackendAPI.Application.Dtos;
using ACBackendAPI.Application.Interfaces.IRepositories;
using ACBackendAPI.Application.Interfaces.IServices;
using ACBackendAPI.Application.Validators;
using ACBackendAPI.Domain.Entities;
using ACBackendAPI.Persistence.Context;
using ACBackendAPI.Persistence.Repositories.Repository;
using ACBackendAPI.Persistence.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using ACBackendAPI.Domain.Settings.Email;
using ACBackendAPI.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy => 
          policy.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
});

// Add Swagger for API documentation and JWT support
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ACBackendAPI", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Enter 'Bearer {your_token_here}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            new string[] { }
        }
    });
});

// Add FluentValidation validators
builder.Services.AddValidatorsFromAssemblyContaining<StudentRegistrationDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateProgrammeDtoValidator>();
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configure EF Core to use PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Identity and Authorization services
builder.Services.AddAuthorization()
    .AddIdentityApiEndpoints<ApplicationUser>()
    .AddRoles<IdentityRole<Guid>>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Register repository and services
builder.Services.AddScoped(typeof(IAsyncRepository<,>), typeof(EfRepository<,>));
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProgrammeService, ProgrammeService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IJwtService, JwtService>();

// Register DTO validators
builder.Services.AddTransient<IValidator<StudentRegistrationDto>, StudentRegistrationDtoValidator>();
builder.Services.AddTransient<IValidator<AdminRegistrationDto>, AdminRegistrationDtoValidator>();
builder.Services.AddTransient<IValidator<GuardianRegistrationDto>, GuardianRegistrationDtoValidator>();
builder.Services.AddTransient<IValidator<AcademicInformationRegistrationDto>, AcademicInformationDtoValidator>();
builder.Services.AddTransient<IValidator<LoginDto>, LoginDtoValidator>();
builder.Services.AddTransient<IValidator<CreateProgrammeDto>, CreateProgrammeDtoValidator>();
builder.Services.AddTransient<IValidator<UpdateProgrammeDto>, UpdateProgrammeDtoValidator>();

// Configure JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))
        };
    });

builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddScoped<IMailService, MailService>();

var app = builder.Build();

// Automatically apply migrations
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var dbContext = services.GetRequiredService<AppDbContext>();
    await dbContext.Database.MigrateAsync();

    var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
    await RoleSeeder.SeedRolesAsync(roleManager);

    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var seeder = new DataSeeder(dbContext, userManager, roleManager);
    await seeder.SeedAdminAsync();
}

// Configure middleware pipeline
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "ACBackendAPI v1");
    options.DocumentTitle = "AC Backend API Docs";
});

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
