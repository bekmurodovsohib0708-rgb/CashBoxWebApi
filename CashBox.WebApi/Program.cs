using CashBox.Service.Configurations;
using CashBox.Service.Integrations.UzasboServices;
using CashBox.Service.Integrations.WeatherServices;
using CashBox.Service.Services;
using CashBox.Service.Services.AuthService;
using CashBox.Service.Services.ConractorAccountServices;
using CashBox.Service.Services.ContractorService;
using CashBox.Service.Services.CorrencyRateServices;
using CashBox.Service.Services.CorrencyServices;
using CashBox.Service.Services.DistrictServices;
using CashBox.Service.Services.DocumentReportServices;
using CashBox.Service.Services.ExelServices;
using CashBox.Service.Services.IncomeDocumentServices;
using CashBox.Service.Services.NewFolder;
using CashBox.Service.Services.OrganizationServices;
using CashBox.Service.Services.OutcomeDocumentService;
using CashBox.Service.Services.OutcomeDocumentServices;
using CashBox.Service.Services.ProductServices;
using CashBox.Service.Services.RegionServices;
using CashBox.Service.Services.RoleServices;
using CashBox.Service.Services.SupplierServices;
using CashBox.Service.Services.UserRoleService;
using CashBox.Service.Services.UserServices;
using CashBox.WebApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Repository.Data;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

AppSettings.Init(builder.Configuration.Get<AppSettings>());
builder.Services.AddSingleton(AppSettings.Instance.UzasboSetting);

builder.Services.AddControllers();

builder.Services.AddCors(options =>  // UIning ishlashi uchun http
{
    options.AddPolicy("AllowLocalhostFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://127.0.0.1:5173", "http://localhost:5174", "http://127.0.0.1:5174", "http://localhost:5175", "http://127.0.0.1:5175", "http://localhost:5177", "http://127.0.0.1:5177")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<AccountService>();

var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true
        };
    });



builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRegionService, RegionService>();
builder.Services.AddScoped<IOrganizationService, OrganizationService>();
builder.Services.AddScoped<IDistrictService, DistrictService>();
builder.Services.AddScoped<ICurrencyRateService, CurrencyRateService>();
builder.Services.AddScoped<ICurrencyService, CurrencyService>();
builder.Services.AddScoped<IContractorService, ContractorService>();
builder.Services.AddScoped<IContratorAccountService, ContratorAccountService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IUserRoleService, UserRoleService>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IIncomeDocumentService, IncomeDocumentService>();
builder.Services.AddScoped<IOutcomeDocumentService, OutcomeDocumentService>();
builder.Services.AddScoped<IDocumentReportService, DocumentReportService>();

builder.Services.AddScoped<IExelService, ExelService>();
builder.Services.AddHttpClient<IWeatherService, WeatherService>();
builder.Services.AddHttpClient<IUzasboService, UzasboService>();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT tokenni kiriting: Bearer {token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.ConfigureServiceApplication();

var app = builder.Build();

app.UseCors("AllowLocalhostFrontend");
app.UseAuthentication();
app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
