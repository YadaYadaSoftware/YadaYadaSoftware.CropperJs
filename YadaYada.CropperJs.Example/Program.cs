using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using YadaYada.CropperJs;
using YadaYada.CropperJs.Example;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddSingleton<ExampleJsInterop>();
builder.Services.AddSingleton<CropperInterop>();
//builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddProvider(new ));

await builder.Build().RunAsync();
