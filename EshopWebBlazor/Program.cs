using EshopWebBlazor;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
var app = builder.Build();
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7200/api/")
});

//builder.Services.AddControllers()
//    .AddJsonOptions(options =>
//    {
//        // To sprawi, że Enumy będą widoczne jako "Customer", a nie 0
//        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
//    });

builder.Services.AddMudServices();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<CustomAuthStateProvider>());
await builder.Build().RunAsync();
