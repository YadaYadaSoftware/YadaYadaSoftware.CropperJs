using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace YadaYada.CropperJs;

public class CropperFactory
{
    private readonly IJSRuntime jSRuntime;
    private readonly Lazy<Task<IJSObjectReference>> _moduleTask;

    public CropperFactory(IJSRuntime jSRuntime)
    {
        this.jSRuntime = jSRuntime;
        _moduleTask = new(() => jSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/YadaYada.CropperJs/cropper.js").AsTask());
    }

    public async Task<CropperInstance> CreateCropperAsync(ElementReference imageReference, Options options)
    {
        var objRef = DotNetObjectReference.Create(options);
        var cropperWrapper = await jSRuntime.InvokeAsync<IJSInProcessObjectReference>("import", "./_content/YadaYada.CropperJs/cropperWrapper.js");
        var jSInstance = await cropperWrapper.InvokeAsync<IJSObjectReference>("createCropper", imageReference, options, objRef);
        return new CropperInstance(jSInstance, objRef, cropperWrapper);
    }
}
