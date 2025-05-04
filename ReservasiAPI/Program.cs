using ReservasiAPI.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Tambahkan CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy => policy
            .WithOrigins("http://localhost:3000") // ganti jika frontend kamu jalan di port lain
            .AllowAnyHeader()
            .AllowAnyMethod());
});

builder.Services.AddDbContext<ReservasiDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ReservasiDB"))
);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Gunakan CORS sebelum routing
app.UseCors("AllowReactApp");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
