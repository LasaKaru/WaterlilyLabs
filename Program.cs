
using Microsoft.EntityFrameworkCore;
using WaterlilyLabs.Data;
using WaterlilyLabs.Repositories;
using WaterlilyLabs.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Configure DbContext
builder.Services.AddDbContext<WaterlilyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Configure Repositories
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IPublicHolidayRepository, PublicHolidayRepository>();

// 3. Configure Caching Service
builder.Services.AddMemoryCache(); 
builder.Services.AddSingleton<ICachingService, CachingService>();

// 4. Configure Services
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IWorkingDaysService, WorkingDaysService>();


builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
