using Microsoft.EntityFrameworkCore;
using restapiapp.Data;

var MyAllow = "_MyAllow";
var builder = WebApplication.CreateBuilder(args);

//connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

//db
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Add services to the container.

builder.Services.AddCors(options => {
    options.AddPolicy(
        name: MyAllow,
        policy =>
        {
            policy.WithOrigins("http://localhost:5173", "http://localhost:7105").AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().SetIsOriginAllowedToAllowWildcardSubdomains().WithExposedHeaders("Referrer-Policy"); ;
        }
        );
});
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
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

app.UseHttpsRedirection();
app.UseCors(MyAllow);
app.UseSession();
app.UseAuthorization();
app.MapControllers();

app.Run();

