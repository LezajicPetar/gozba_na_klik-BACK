using System.Text;
using System.Text.Json.Serialization;
using gozba_na_klik.Data;
using gozba_na_klik.Mapping;
using gozba_na_klik.Middlewear;
using gozba_na_klik.Model;
using gozba_na_klik.Repository;
using gozba_na_klik.Service;
using gozba_na_klik.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Filters;

namespace gozba_na_klik
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers()
                .AddJsonOptions(o =>
                    o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new() { Title = "Gozba Na Klik API", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Enter a valid JWT token below. Example: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
            });

            builder.Services.AddDbContext<GozbaDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
            
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<UserRepository>();
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<AllergenRepository>();
            builder.Services.AddScoped<UserAllergenRepository>();
            builder.Services.AddScoped<UserAllergenService>();

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IRepository<User>, UserRepository>(); 
            builder.Services.AddScoped<IRestaurantRepository, RestaurantRepository>();
            builder.Services.AddScoped<IRepository<Restaurant>, RestaurantRepository>();

            builder.Services.AddScoped<IAdminUserService, AdminUserService>();
            builder.Services.AddScoped<IAdminRestaurantService, AdminRestaurantService>();

            builder.Services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<RestaurantProfile>();
                cfg.AddProfile<UserProfile>();
            });

            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .Filter.ByExcluding(Matching.FromSource("Microsoft"))
                .Filter.ByExcluding(Matching.FromSource("System"))
                .Filter.ByIncludingOnly(Matching.FromSource("gozba_na_klik"))                
                .CreateLogger();

            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog(logger);

            builder.Services.AddTransient<ExceptionHandlingMiddleware>();

            builder.Services.AddCors(opt =>
            {
                opt.AddPolicy("Front", p =>
                    p.WithOrigins("http://localhost:5173")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials());
            });

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var jwtKey = builder.Configuration["Jwt:Key"] ?? "dev-placeholder-key";
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "dev-issuer",
                        ValidAudience = builder.Configuration["Jwt:Audience"] ?? "dev-audience",
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                        ValidateLifetime = true,
                    };
                });

            builder.Services.AddAuthorization();

            builder.Services.AddSingleton<TokenService>();
            //dodato ako zelimo da upload-ujemo fajlove vece od 10MB
            builder.Services.Configure<FormOptions>(o =>
            {
                o.MultipartBodyLengthLimit = 10 * 1024 * 1024; // 10 MB
            });

            var app = builder.Build();

            app.UseMiddleware<ExceptionHandlingMiddleware>();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("Front");
            app.UseAuthentication();
            app.UseAuthorization();
            
            //bitno da bi /uploads/ radilo
            app.UseStaticFiles();

            app.MapControllers();

            app.Run();
        }
    }
}
