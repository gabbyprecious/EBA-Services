using FinquestTest.Models;
using FinquestTest.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<UserStoreDatabaseSettings>(
    builder.Configuration.GetSection("UserStoreDatabase"));

builder.Services.AddSingleton<UsersService>();
builder.Services.AddSingleton<IMessageService, MessageService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseUrls("http://*:5000");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


