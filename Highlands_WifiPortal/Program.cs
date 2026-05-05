using Highlands_WifiPortal.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Đăng ký Session Service
builder.Services.AddSession();

// Add services to the container.
builder.Services.AddControllersWithViews();

// Đăng ký các Mock Services để Controller chạy được
builder.Services.AddScoped<OtpService>();
builder.Services.AddScoped<ZaloApiService>();

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

// 2. GỌI MIDDLEWARE SESSION VÀO ĐÂY (Bắt buộc phải nằm sau UseRouting)
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();