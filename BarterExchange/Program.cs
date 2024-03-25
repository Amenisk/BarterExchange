using BarterExchange.Data;
using BarterExchange.Data.Classes;
using BarterExchange.Data.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MongoDB.Driver;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<FileSystemService>();
builder.Services.AddSingleton<ExchangeOrderService>();
builder.Services.AddSingleton<ItemService>();
builder.Services.AddMudServices();
Database.client = new MongoClient(builder.Configuration["ConnectionStrings:Mongo"]!);
Database.database = Database.client.GetDatabase("BarterExchangeService");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
