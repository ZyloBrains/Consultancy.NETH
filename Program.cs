using Microsoft.EntityFrameworkCore;
using Consultancy.Data;
using Consultancy.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddRazorPages();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddScoped<ITeacherService, TeacherService>();
builder.Services.AddScoped<IBlogService, BlogService>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<ITestimonialService, TestimonialService>();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<ILocalizationService, LocalizationService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseSession();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();