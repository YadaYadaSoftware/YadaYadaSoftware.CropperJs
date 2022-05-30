using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace YadaYada.CropperJs;

public class CropperFactory
{
    private readonly IJSRuntime jSRuntime;
    private readonly Lazy<Task<IJSObjectReference>> _moduleTask;
    private readonly ILogger _logger;

    public CropperFactory(IJSRuntime jSRuntime, ILoggerProvider loggerProvider)
    {
        this.jSRuntime = jSRuntime;
        _moduleTask = new(() => jSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/YadaYada.CropperJs/cropper.js").AsTask());
        _logger = loggerProvider.CreateLogger(this.GetType().FullName!);
    }

    public async Task<CropperInstance> CreateCropperAsync(ElementReference imageReference, Options options)
    {
        using (_logger.BeginScope(nameof(CreateCropperAsync)))
        using(_logger.BeginScope("{imageReference}.{imageReferenceId}='{Id}'", nameof(imageReference), nameof(imageReference.Id), imageReference.Id))
        {
            DotNetObjectReference<Options> objRef = DotNetObjectReference.Create(options);
            
            var cropperWrapper = await jSRuntime.InvokeAsync<IJSInProcessObjectReference>("import", "./_content/YadaYada.CropperJs/cropperWrapper.js");
            var jSInstance = await cropperWrapper.InvokeAsync<IJSObjectReference>("createCropper", imageReference, objRef);
            var cropperInstance = new CropperInstance(jSInstance, objRef, cropperWrapper);
            _logger.LogInformation("Created:{0}", cropperInstance.GetHashCode());
            return cropperInstance;

        }
    }
}
