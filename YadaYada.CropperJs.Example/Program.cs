using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using YadaYada.CropperJs;
using YadaYada.CropperJs.Example;
using YadaYadaSoftware.CropperJs;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddSingleton<ExampleJsInterop>();
builder.Services.AddCropperServices();
//builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddProvider(new ));

await builder.Build().RunAsync();
