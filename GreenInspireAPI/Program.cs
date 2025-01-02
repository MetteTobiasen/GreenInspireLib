using GreenInspireLib.BusinessLogicLayer;
using GreenInspireLib.Models;
using GreenInspireLib.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowAll",
                              policy =>
                              {
                                  policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                              });
});
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<GreenInspireContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GreenInspireLocalDB")));

builder.Services.AddTransient<CategorySqlService, CategorySqlService>();    
builder.Services.AddTransient<NewsfeedSqlService, NewsfeedSqlService>();    
builder.Services.AddTransient<NewsfeedLogic,  NewsfeedLogic>();

var app = builder.Build();

app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
