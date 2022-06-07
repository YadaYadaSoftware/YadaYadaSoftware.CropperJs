using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace YadaYadaSoftware.CropperJs;

public class CropperFactory
{
    private readonly IJSRuntime _jsRuntime;
    private readonly Lazy<Task<IJSObjectReference>> _moduleTask;
    private readonly ILogger _logger;

    public CropperFactory(IJSRuntime jsRuntime, ILoggerProvider loggerProvider)
    {
        this._jsRuntime = jsRuntime;
        _moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/YadaYada.CropperJs/cropper.js").AsTask());
        _logger = loggerProvider.CreateLogger(this.GetType().FullName!);
    }

    public async Task<CropperInstance> CreateCropperAsync(ElementReference imageReference, Options options)
    {
        using (_logger.BeginScope(nameof(CreateCropperAsync)))
        using(_logger.BeginScope("{imageReference}.{imageReferenceId}='{Id}'", nameof(imageReference), nameof(imageReference.Id), imageReference.Id))
        {
            DotNetObjectReference<Options> objRef = DotNetObjectReference.Create(options);

            string path = $"./_content/{typeof(Cropper).Assembly.GetName().Name}/cropperWrapper.js";
            var cropperWrapper = await _jsRuntime.InvokeAsync<IJSInProcessObjectReference>("import", path);
            var jSInstance = await cropperWrapper.InvokeAsync<IJSObjectReference>("createCropper", imageReference, objRef, options);
            var cropperInstance = new CropperInstance(jSInstance, objRef, cropperWrapper);
            _logger.LogInformation("Created:{0}", cropperInstance.GetHashCode());
            return cropperInstance;

        }
    }
}
