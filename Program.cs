
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.PropertyNameCaseInsensitive = false;
    options.SerializerOptions.PropertyNamingPolicy = null;
    options.SerializerOptions.WriteIndented = true;

});
builder.Services.AddRazorPages();
builder.Services.AddSingleton<HandleBlockService>();
builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
builder.Services.AddSingleton<Chain>();
//builder.Services.AddHostedService<HighPerformanceServer>();
var app = builder.Build();

var chain = app.Services.GetRequiredService<Chain>();
try
{
    string[] fileEntries = Directory.GetFiles(Path.Join(Environment.CurrentDirectory, "Files"));
    foreach (string fileName in fileEntries)
    {
        chain.AddBlock(File.ReadAllText(fileName));
    }
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}
//chain.AddBlock("First block");

app.MapGet("/api/chain", () => chain.TheChain);
app.UseStaticFiles();
app.MapRazorPages();
app.Run();