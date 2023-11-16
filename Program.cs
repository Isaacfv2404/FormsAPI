using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FormsAPI.Data;
using FormsAPI.Services;
using Microsoft.AspNetCore.Authentication;
using FormsAPI.Security;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<FormsAPIContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("FormsAPIContext") ?? throw new InvalidOperationException("Connection string 'FormsAPIContext' not found.")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthHandler>("BasicAuthentication", null);
builder.Services.AddCors(p => p.AddPolicy("MyCorsPolicy", build =>
{
    build.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
}));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("MyCorsPolicy");
app.MapControllers();

app.Run();
