using PHIRedationApplication.Server.Services;
using PHIRedationApplication.Server.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddLogging();
builder.Services.AddScoped<IWriteFileService, WriteFileService>();
builder.Services.AddScoped<IReadFileService, ReadFileService>();
builder.Services.AddScoped<IDirectoryService, DirectoryService>();
builder.Services.AddScoped<IPhiRedactionService, PhiRedactionService>();
builder.Services.AddScoped<PhiRedactorService>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseDefaultFiles();
app.MapStaticAssets();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapFallbackToFile("/index.html");

app.Run();