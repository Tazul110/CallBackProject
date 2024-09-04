using AirlineAPI.Data;
using AirlineAPI.DependencyInjection;
using AirlineAPI.Interfaces;
using AirlineAPI.Middlewire;
using AirlineAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Settings
var s3Settings = new AwsS3Settings();
builder.Configuration.Bind("awsS3", s3Settings);
builder.Services.AddSingleton(s3Settings);


/*var appSetting = new AppSettings();
builder.Configuration.Bind("AppSettings", appSetting);
builder.Services.AddSingleton(appSetting);*/
#endregion Settings

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICallbackService, CallbackService>();
builder.Services.AddDependencyInjection();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<IpWhitelistMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
