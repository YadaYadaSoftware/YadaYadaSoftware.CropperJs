﻿using Microsoft.JSInterop;

namespace YadaYadaSoftware.CropperJs;

public class CropperInstance : IDisposable
{
    private readonly IJSObjectReference _cropperJsInstance;
    private readonly DotNetObjectReference<Options> _objRef;
    private readonly IJSInProcessObjectReference _cropperWrapper;

    public CropperInstance(IJSObjectReference cropperJsInstance, DotNetObjectReference<Options> objRef, IJSInProcessObjectReference cropperWrapper)
    {
        this._cropperJsInstance = cropperJsInstance;
        this._objRef = objRef;
        this._cropperWrapper = cropperWrapper;
    }
    public State State
    {
        get { return _cropperWrapper.Invoke<State>("getStateOfInstance", _cropperJsInstance); }
    }
    public async Task ForceUpdate() => await _cropperWrapper.InvokeVoidAsync("forceUpdate");
    public async Task<State> Update() => await _cropperWrapper.InvokeAsync<State>("updateOnInstance", _cropperJsInstance);
    public async Task<State> SetOptions(Options options) => await _cropperWrapper.InvokeAsync<State>("setOptionsOnInstance", _cropperJsInstance, options, _objRef);
    public async Task Destroy() => await _cropperJsInstance.InvokeVoidAsync("destroy");

    public async Task Zoom(decimal ratio) => await _cropperWrapper.InvokeVoidAsync("zoom", _cropperJsInstance, ratio);
    public async Task DragMode(DragModeEnum mode)
    {
        const string none = "none";
        const string move = "move";
        const string crop = "crop";

        switch (mode)
        {
            case DragModeEnum.Cropper:
                await _cropperWrapper.InvokeVoidAsync("setDragMode", _cropperJsInstance, crop);
                break;
            case DragModeEnum.Image:
                await _cropperWrapper.InvokeVoidAsync("setDragMode", _cropperJsInstance, move);
                break;
            case DragModeEnum.None:
                await _cropperWrapper.InvokeVoidAsync("setDragMode", _cropperJsInstance, none);
                break;
            default: 
                throw new ArgumentOutOfRangeException(nameof(mode));
        }
    }


    public void Dispose()
    {
        _objRef?.Dispose();
        _cropperWrapper?.Dispose();
    }

    public async Task SetCropAsync(CropData data)
    {
        await _cropperWrapper.InvokeVoidAsync("setData", _cropperJsInstance, data);
    }

    public async Task ZoomTo(decimal zoomLevel)
    {
        await _cropperWrapper.InvokeVoidAsync("zoomTo", _cropperJsInstance, zoomLevel);
    }
    public async Task RotateTo(decimal degrees)
    {
        await _cropperWrapper.InvokeVoidAsync("rotateTo", _cropperJsInstance, degrees);
    }
    public async Task Rotate(decimal degrees)
    {
        await _cropperWrapper.InvokeVoidAsync("rotate", _cropperJsInstance, degrees);
    }
}