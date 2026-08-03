using LuckyExpenses.Application.DependencyInjection;
using LuckyExpenses.Infrastructure.DependencyInjection;
using LuckyExpenses.WebAPI.Config.Filters;
using LuckyExpenses.WebAPI.Config.Jwt;
using LuckyExpenses.WebAPI.Config.Options;
using LuckyExpenses.WebAPI.Middlewares;
using Microsoft.OpenApi;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);

builder.Services.AddControllers(options =>
{
    options.Filters.Add<GlobalResponseFilter>();
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddProblemDetails();

builder.Services.AddCors(options =>
{
    options.AddPolicy("PoliticaCors", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingresa el token JWT (sin prefijo 'Bearer ') para autenticarte."
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = new List<string>()
    });
});

builder.Services.AddExceptionHandler<ExceptionHandler>();
builder.Services.AddConfiguredOptions(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddJwtAuthentication();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Lucky Expenses API V1");
    });
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("Lucky Expenses API Documentation");
        options.OpenApiRoutePattern = "/swagger/v1/swagger.json";
    });
}
else
{
    app.UseHsts();
}

app.Use(async (context, next) =>
{
    if (!context.Response.HasStarted)
    {
        context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
        context.Response.Headers.Append("X-Frame-Options", "DENY");
        context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");
        context.Response.Headers.Append("Referrer-Policy", "no-referrer");
    }
    await next();
});

app.UseCors("PoliticaCors");
app.UseExceptionHandler(app => { });
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/", () => "LuckyExpenses Activo");

app.Run();
