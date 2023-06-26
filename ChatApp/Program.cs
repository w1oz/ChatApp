using ChatApp.Model;
using ChatApp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.WebSockets;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using SignalRChat.Hubs;
using System.Diagnostics;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
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
    options.AddPolicy(MyAllowSpecificOrigins,
                          policy =>
                          {
                              policy.AllowAnyOrigin()
                                                  .AllowAnyHeader()
                                                  .AllowAnyMethod();
                          });
}); builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(option =>
                {
                    option.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "http://192.168.3.12:8080",
                        ValidAudience = "http://192.168.3.12:8080",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("64A63153-11C1-4919-9133-EFAF99A9B456")),
                        ClockSkew = TimeSpan.Zero
                    };
                });
builder.Services.AddAuthorization();
Console.WriteLine("test1");


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(MyAllowSpecificOrigins);
app.UseAuthentication();
app.UseAuthorization();
//app.UseHttpsRedirection();
//app.UseMiddleware<WebSocketMiddleware>();
//app.UseAuthentication();
//app.UseAuthorization();
app.MapControllers().RequireAuthorization(); 

app.MapHub<ChatHub>("/chat", map =>
{
    Console.WriteLine(map);
}).RequireAuthorization();
app.Run();
