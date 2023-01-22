using Dashboard.Blazor.Data;
using Dashboard.Blazor.Hubs;
using Dashboard.Observability;
using Microsoft.AspNetCore.ResponseCompression;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddDashboardLogging();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Added
builder.Services.AddOptions<TornConfiguration>()
    .BindConfiguration("Torn")
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddSingleton<ChatService>();
builder.Services.AddSignalR();
builder.Services.AddResponseCompression(options =>
{
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/octet-stream" });
});

var app = builder.Build();

// Added
app.UseResponseCompression();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();

// Added
app.MapHub<ChatStreamHub>("/chathub");

app.MapFallbackToPage("/_Host");

app.Run();
