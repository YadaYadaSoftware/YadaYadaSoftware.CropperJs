using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace YadaYada.CropperJs;

public class CropperInterop : IAsyncDisposable
{
    private readonly IJSRuntime _jsRuntime;
    private readonly Lazy<Task<IJSObjectReference>> _moduleTask;

    public CropperInterop(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
        _moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/YadaYada.CropperJs/cropper.js").AsTask());
    }

    public async Task<CropperInstance> CreatePopperAsync(ElementReference imageReference, Options options)
    {
        var objRef = DotNetObjectReference.Create(options);
        var cropperWrapper = await _jsRuntime.InvokeAsync<IJSInProcessObjectReference>("import", "./_content/YadaYada.CropperJs/cropperWrapper.js");
        var jSInstance = await cropperWrapper.InvokeAsync<IJSObjectReference>("createCropper", imageReference, options, objRef);
        return new(jSInstance, objRef, cropperWrapper);
    }

    public async ValueTask DisposeAsync()
    {
        if (_moduleTask.IsValueCreated)
        {
            var module = await _moduleTask.Value;
            await module.DisposeAsync();
        }
    }
}

public class Options
{

}

public class State
{

}

public class CropperInstance : IDisposable
{
    private readonly IJSObjectReference jSInstance;
    private readonly DotNetObjectReference<Options> objRef;
    private readonly IJSInProcessObjectReference popperWrapper;

    public CropperInstance(IJSObjectReference jSInstance, DotNetObjectReference<Options> objRef, IJSInProcessObjectReference popperWrapper)
    {
        this.jSInstance = jSInstance;
        this.objRef = objRef;
        this.popperWrapper = popperWrapper;
    }
    //public State State
    //{
    //    get { return popperWrapper.Invoke<State>("getStateOfInstance", jSInstance); }
    //}
    //public async Task ForceUpdate() => await jSInstance.InvokeVoidAsync("forceUpdate");
    //public async Task<State> Update() => await popperWrapper.InvokeAsync<State>("updateOnInstance", jSInstance);
    //public async Task<State> SetOptions(Options options) => await popperWrapper.InvokeAsync<State>("setOptionsOnInstance", jSInstance, options, objRef);
    //public async Task Destroy() => await jSInstance.InvokeVoidAsync("destroy");

    public void Dispose()
    {
        objRef?.Dispose();
    }

}