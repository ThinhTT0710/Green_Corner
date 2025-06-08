using AutoMapper;
using GreenCorner.EventAPI.Data;
using GreenCorner.EventAPI.Extensions;
using GreenCorner.EventAPI.Mapping;
using GreenCorner.EventAPI.Repositories;
using GreenCorner.EventAPI.Repositories.Interface;
using GreenCorner.EventAPI.Services;
using GreenCorner.EventAPI.Services.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<GreenCornerEventContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
IMapper mapper = MappingConfig.EventMaps().CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
	options.AddSecurityDefinition(name: JwtBearerDefaults.AuthenticationScheme, securityScheme: new OpenApiSecurityScheme
	{
		Name = "Authorization",
		Description = " Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
		In = ParameterLocation.Header,
		Type = SecuritySchemeType.ApiKey,
		Scheme = "Bearer"
	});
	options.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type = ReferenceType.SecurityScheme,
					Id = JwtBearerDefaults.AuthenticationScheme
				}
			},
			new string[] {}
		}
	});
});

builder.AddAppAuthetication();
builder.Services.AddAuthorization();

builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<ILeaderReviewRepository, LeaderReviewRepository>();
builder.Services.AddScoped<ILeaderReviewService, LeaderReviewService>();
builder.Services.AddScoped<IEventReviewRepository, EventReviewRepository>();
builder.Services.AddScoped<IEventReviewService, EventReviewService>()

builder.Services.AddScoped<ITrashEventRepository, TrashEventRepository>();
builder.Services.AddScoped<ITrashEventService, TrashEventService>();

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

app.MapControllers();

app.Run();
