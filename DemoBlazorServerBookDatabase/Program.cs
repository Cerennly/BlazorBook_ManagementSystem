using DemoBlazorServerBookDatabase.BookServices;
using DemoBlazorServerBookDatabase.Components;
using DemoBlazorServerBookDatabase.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddControllers();
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient("LocalApi", client => client.BaseAddress = new Uri("https://localhost:7074/"));

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Database connection string not found"));
});

builder.Services.AddScoped<IBookService, BookService>();  //interface eklendik



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error", createScopeForErrors: true);
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();


app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Eðer kimlik doðrulama kullanýyorsanýz
app.UseAuthorization(); // Eðer yetkilendirme kullanýyorsanýz

app.UseAntiforgery();  // saldiriya karsi bir guvenlik ornegidur-forgery means that sahtecilik

app.MapBlazorHub();
app.MapRazorComponents<App>();  //tamamen blazor icin home fallbackto page yapisi
app.MapRazorPages();
app.MapControllers();

app.Run();
