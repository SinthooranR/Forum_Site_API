using Forum_Application_API;
using Forum_Application_API.Data;
using Forum_Application_API.Interfaces;
using Forum_Application_API.Methods;
using Forum_Application_API.Models;
using Forum_Application_API.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Hosting.Internal;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//Fixes Object Cycle Errors
builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:3000") 
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddScoped<IUserInterface, UserRepository>();
builder.Services.AddScoped<IThreadInterface, ThreadRepository>();
builder.Services.AddScoped<ICommentInterface, CommentRepository>();
builder.Services.AddScoped<JwtGenerator>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//Add DB Context
//This checks appsettings.json

string connectionName = builder.Environment.IsDevelopment() ? "DefaultConnection" : "DefaultConnection";
string connectionString = builder.Configuration.GetConnectionString(connectionName);
builder.Services.AddDbContext<DataContext>(options => {
    options.UseSqlServer(connectionString);

    options.EnableSensitiveDataLogging();
});

builder.Services.AddIdentity<User, IdentityRole<int>>(options =>
{
    options.User.RequireUniqueEmail = true; 
    options.SignIn.RequireConfirmedEmail = false; 
})
.AddEntityFrameworkStores<DataContext>()
.AddDefaultTokenProviders();

var app = builder.Build();


// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
