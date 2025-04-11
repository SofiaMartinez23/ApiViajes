using ApiViajes.Data;
using ApiViajes.Helpers;
using ApiViajes.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

HelperCryptography.Initialize(builder.Configuration);
builder.Services.AddHttpContextAccessor();

// Registrar HelperUsuarioToken
builder.Services.AddScoped<HelperUsuarioToken>(); 
HelperActionServicesOAuth helper = new HelperActionServicesOAuth(builder.Configuration);
builder.Services.AddSingleton<HelperActionServicesOAuth>(helper);
builder.Services.AddAuthentication(helper.GetAuthenticateSchema())
    .AddJwtBearer(helper.GetJwtBearerOptions());

// Add services to the container.
string connectionString = builder.Configuration.GetConnectionString("SqlAzure");
builder.Services.AddTransient<RepositoryAuth>();
builder.Services.AddTransient<RepositoryUsuarios>();
builder.Services.AddTransient<RepositoryLugar>();
builder.Services.AddTransient<RepositoryComentarios>();
builder.Services.AddTransient<RepositoryChats>();
builder.Services.AddTransient<RepositoryFavoritos>();
builder.Services.AddTransient<RepositorySeguidos>();
builder.Services.AddDbContext<ViajesContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

app.MapOpenApi();
app.UseHttpsRedirection();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/v1.json", "Api Viajes");
    options.RoutePrefix = "";
});
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
