using ReservasiAPI.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Tambahkan CORS policy
builder.Services.AddCors(options => {
    options.AddPolicy("AllowReactApp", policy => {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Tambahkan DB Context
builder.Services.AddDbContext<ReservasiDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ReservasiDB"))
);

// Tambahkan Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ReservasiDbContext>()
    .AddDefaultTokenProviders();

// Tambahkan autentikasi berbasis cookie
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.LoginPath = "/api/user/login";
    options.AccessDeniedPath = "/access-denied";
    options.SlidingExpiration = true;
});

// ✅ Tambahkan HttpClient agar bisa dipakai untuk call Midtrans
builder.Services.AddHttpClient();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Gunakan CORS sebelum routing
app.UseStaticFiles();
app.UseCors("AllowReactApp");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Middleware untuk autentikasi dan otorisasi
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
        