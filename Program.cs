using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Mundial.Models;
using Mundial.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("AppDbContext");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//builder.Services.AddDefaultIdentity<AppUsers>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddIdentity<AppUsers, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
builder.Services.AddAuthentication();
builder.Services.AddControllersWithViews();
//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
//    options.AddPolicy("UserAndAdmin", policy => policy.RequireRole("Admin").RequireRole("User"));
//    options.AddPolicy("Admin_CreateAccess", policy => policy.RequireRole("Admin").RequireClaim("create", "True"));
//    options.AddPolicy("Admin_Create_Edit_DeleteAccess", policy => policy.RequireRole("Admin").RequireClaim("create", "True")
//    .RequireClaim("edit", "True")
//    .RequireClaim("Delete", "True"));
//    options.AddPolicy("Admin_Create_Edit_DeleteAccess_OR_SuperAdmin", policy => policy.RequireAssertion(context =>
//    context.User.IsInRole("Admin") && context.User.HasClaim(c => c.Type == "Create" && c.Value == "True")
//                        && context.User.HasClaim(c => c.Type == "Edit" && c.Value == "True")
//                        && context.User.HasClaim(c => c.Type == "Delete" && c.Value == "True")
//                        || context.User.IsInRole("SuperAdmin")));
//});

builder.Services.Configure<IdentityOptions>(opt =>
{
    opt.Password.RequiredLength = 5;
    opt.Password.RequireLowercase = true;
    opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(10);
    opt.Lockout.MaxFailedAccessAttempts = 5;
});
builder.Services.AddRazorPages();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
