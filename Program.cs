
using gozba_na_klik.Data;
using gozba_na_klik.Middlewear;
using gozba_na_klik.Repository;
using gozba_na_klik.Service.External;
using gozba_na_klik.Service.Implementations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Filters;
using System.Text;
using System.Text.Json.Serialization;



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

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<GozbaDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
            
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<UserRepository>();
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<AllergenRepository>();
            builder.Services.AddScoped<UserAllergenRepository>();
            builder.Services.AddScoped<UserAllergenService>();
            builder.Services.AddScoped<RestaurantRepository>();

            builder.Services.AddAutoMapper(cfg =>
            {
                //cfg.AddProfile<MappingProfile>(); PRIMER ZA DODAVANJE PROFILA
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

            builder.Services.AddScoped<gozba_na_klik.Model.Interfaces.ICourierRepository, gozba_na_klik.Repository.CourierRepository>();
            builder.Services.AddScoped<gozba_na_klik.Service.Interfaces.ICourierService, gozba_na_klik.Service.Implementations.CourierService>();

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
