using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Consultancy.Data;
using Consultancy.Models;
using Consultancy.Services;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 6;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

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
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IBlogService, BlogService>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<ITestimonialService, TestimonialService>();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<ILocalizationService, LocalizationService>();
builder.Services.AddScoped<IGoogleDriveService, GoogleDriveService>();
builder.Services.AddScoped<IDocumentVerificationService, DocumentVerificationService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.MapGet("/seed-all", async (ApplicationDbContext db) =>
{
    // Seed Countries first
    if (!await db.Countries.AnyAsync())
    {
        db.Countries.AddRange(new[] 
        { 
            new Consultancy.Models.Entities.Country { Name = "USA" },
            new Consultancy.Models.Entities.Country { Name = "UK" },
            new Consultancy.Models.Entities.Country { Name = "Canada" }
        });
        await db.SaveChangesAsync();
    }
    
    // Seed Categories
    if (!await db.Categories.AnyAsync())
    {
        db.Categories.AddRange(new[] 
        { 
            new Consultancy.Models.Entities.Category { Name = "Technology" },
            new Consultancy.Models.Entities.Category { Name = "Business" }
        });
        await db.SaveChangesAsync();
    }
    
    // Seed Courses
    if (!await db.Courses.AnyAsync())
    {
        var category = await db.Categories.FirstAsync();
        db.Courses.AddRange(new[] 
        { 
            new Consultancy.Models.Entities.Course 
            { 
                Name = "Computer Science", 
                Fees = 1000,
                CategoryId = category.Id,
                CountryId = (await db.Countries.FirstAsync()).Id
            },
            new Consultancy.Models.Entities.Course 
            { 
                Name = "Business Administration", 
                Fees = 1200,
                CategoryId = category.Id,
                CountryId = (await db.Countries.FirstAsync()).Id
            }
        });
        await db.SaveChangesAsync();
    }
    
    // Seed Document Requirements
    if (!await db.DocumentRequirements.AnyAsync())
    {
        db.DocumentRequirements.AddRange(new[]
        {
            new Consultancy.Models.Entities.DocumentRequirement 
            { 
                Name = "Transcript", 
                Description = "Academic transcript", 
                DisplayOrder = 1, 
                IsActive = true, 
                CreatedAt = DateTime.UtcNow 
            },
            new Consultancy.Models.Entities.DocumentRequirement 
            { 
                Name = "Passport", 
                Description = "Valid passport", 
                DisplayOrder = 2, 
                IsActive = true, 
                CreatedAt = DateTime.UtcNow 
            },
            new Consultancy.Models.Entities.DocumentRequirement 
            { 
                Name = "Certificates", 
                Description = "Birth/marriage certificates", 
                DisplayOrder = 3, 
                IsActive = true, 
                CreatedAt = DateTime.UtcNow 
            }
        });
        await db.SaveChangesAsync();
    }
    
    // Seed Students
    if (!await db.Students.AnyAsync())
    {
        var country = await db.Countries.FirstAsync();
        var course1 = await db.Courses.FirstAsync();
        var course2 = await db.Courses.Skip(1).FirstOrDefaultAsync() ?? course1;
        
        db.Students.AddRange(new[]
        {
            new Consultancy.Models.Entities.Student 
            { 
                Name = "John Doe", 
                Email = "john@example.com", 
                Phone = "+1234567890", 
                Address = "123 Main St",
                CountryId = country.Id,
                CourseId = course1.Id
            },
            new Consultancy.Models.Entities.Student 
            { 
                Name = "Jane Smith", 
                Email = "jane@example.com", 
                Phone = "+0987654321", 
                Address = "456 Oak Ave",
                CountryId = country.Id,
                CourseId = course2.Id
            }
        });
        await db.SaveChangesAsync();
        return "All data seeded: Countries, Categories, Courses, Document Requirements, and Students!";
    }
    return "Data already exists!";
});

app.Run();