using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RiPOS.API.Utilities.Helpers;
using RiPOS.Core.Interfaces;
using RiPOS.Core.Services;
using RiPOS.Repository.Interfaces;
using RiPOS.Repository.Repositories;
using RiPOS.Repository.Session;
using RiPOS.Shared.Models.Settings;

namespace RiPOS.API.Utilities.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();

            if (jwtSettings == null)
            {
                throw new Exception("JwtSettings section not found in configuration file");
            }

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        RequireExpirationTime = true,
                        RequireSignedTokens = true,
                        ClockSkew = TimeSpan.FromSeconds(10),
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSettings.Key))
                    };
                });
        }

        public static void ConfigureCors(this IServiceCollection services, IConfiguration configuration)
        {
            var appSettings = configuration.GetSection("AppSettings").Get<AppSettings>();
            
            if (appSettings == null)
            {
                throw new Exception("AppSettings section not found in configuration file");
            }

            var allowedOrigins = appSettings.AllowedCorsOrigins;
            var origins = allowedOrigins.Split(',', StringSplitOptions.RemoveEmptyEntries);
            
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        .WithOrigins(origins)
                        .AllowCredentials()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                );
            });
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "RiPOS API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme { In = ParameterLocation.Header, Description = "Please insert JWT with bearer into field", Name = "Authorization", Type = SecuritySchemeType.ApiKey });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        []
                    }}
                );
                c.OperationFilter<SwaggerFileOperationFilter>();
            });
        }

        public static void ConfigureCoreServices(this IServiceCollection services)
        {
            services.AddTransient<IBrandService, BrandService>();
            services.AddTransient<ICashRegisterService, CashRegisterService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IColorService, ColorService>();
            services.AddTransient<ICustomerService, CustomerService>();
            services.AddTransient<IGenderService, GenderService>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IMemoryCacheService, MemoryCacheService>();
            services.AddTransient<IMiscService, MiscService>();
            services.AddTransient<ISizeService, SizeService>();
            services.AddTransient<IStoreService, StoreService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IVendorService, VendorService>();
        }

        public static void ConfigureRepositories(this IServiceCollection services)
        {
            services.AddScoped<IRepositorySessionFactory, RepositorySessionFactory>();

            services.AddTransient<IBrandRepository, BrandRepository>();
            services.AddTransient<ICashRegisterRepository, CashRegisterRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IColorRepository, ColorRepository>();
            services.AddTransient<ICustomerRepository, CustomerRepository>();
            services.AddTransient<IGenderRepository, GenderRepository>();
            services.AddTransient<IAuthRepository, AuthRepository>();
            services.AddTransient<IMiscRepository, MiscRepository>();
            services.AddTransient<ISizeRepository, SizeRepository>();
            services.AddTransient<IStoreRepository, StoreRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IVendorRepository, VendorRepository>();
        }
    }
}
