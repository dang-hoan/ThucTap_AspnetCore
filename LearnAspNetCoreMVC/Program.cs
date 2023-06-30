using LearnAspNetCoreMVC.CommandHandlers;
using LearnAspNetCoreMVC.CommandHandlers.Category;
using LearnAspNetCoreMVC.Data;
using LearnAspNetCoreMVC.QueryHandlers.Category;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
    ));

builder.Services.AddScoped<AddCommandHandler>();
builder.Services.AddScoped<DeleteCommandHandler>();
builder.Services.AddScoped<UpdateCommandHandler>();
builder.Services.AddScoped<GetAllQueryHandler>();
builder.Services.AddScoped<GetByIdQueryHandler>();
builder.Services.AddScoped<SearchQueryHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
