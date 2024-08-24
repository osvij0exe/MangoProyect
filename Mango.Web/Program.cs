using Mango.Web.Services;
using Mango.Web.Services.IServices;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.AddHttpClient<ICouponServices,CouponServices>();
builder.Services.AddHttpClient<IAuthService,AuthService>();
builder.Services.AddHttpClient<IProductService,ProductService>();
builder.Services.AddHttpClient<IshoppingCartService, ShoppingCartService>();
builder.Services.AddHttpClient<IOrderService, OrderService>();

var setingStrings = builder.Configuration.GetSection("ServiceUrl");

SD.CouponApiBase = setingStrings.GetValue<string>("CouponApi");
SD.AuthApiBase = setingStrings.GetValue<string>("AuthApi");
SD.ProductApiBase = setingStrings.GetValue<string>("ProductApi");
SD.ShoppingCartApiBase = setingStrings.GetValue<string>("ShoppingCartApi");
SD.OrdeApiBase = setingStrings.GetValue<string>("OrderApi");


builder.Services.AddScoped<ITokenProvider, TokenProvider>();
builder.Services.AddScoped<IBaseServices,BaseServices>();
builder.Services.AddScoped<ICouponServices,CouponServices>();
builder.Services.AddScoped<IAuthService,AuthService>();
builder.Services.AddScoped<IProductService,ProductService>();
builder.Services.AddScoped<IshoppingCartService ,ShoppingCartService>();
builder.Services.AddScoped<IOrderService ,OrderService>();


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromHours(10);
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Auth/AccessDenied";
    });

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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
