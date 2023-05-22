using ChatApp.Model;
using ChatApp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.WebSockets;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using SignalRChat.Hubs;
using System.Diagnostics;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Add services to the container.
builder.Services.Configure<DbModel>(builder.Configuration.GetSection("ChatAppDatabase"));
builder.Services.AddSingleton<UserServices>();
builder.Services.AddSingleton<MessageServices>();
builder.Services.AddSignalR();
builder.Services.AddCors(options =>
{
    options.AddPolicy("ClientPermission", policy =>
    {
        policy.AllowAnyHeader()
            .AllowAnyMethod()
            .WithOrigins("https://192.168.3.12:8080", "https://localhost:7088")
            .AllowCredentials();
    });
});

Console.WriteLine("test1");


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
//app.UseMiddleware<WebSocketMiddleware>();
//app.UseAuthentication();
//app.UseAuthorization();
app.MapControllers();
app.UseCors(x => x
   .AllowAnyMethod()
   .AllowAnyHeader()
   .SetIsOriginAllowed(origin => true) // allow any origin  
   .AllowCredentials());
app.MapHub<ChatHub>("/chat", map =>
{
    Console.WriteLine(map);
});
app.Run();
