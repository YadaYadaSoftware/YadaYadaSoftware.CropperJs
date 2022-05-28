using Microsoft.JSInterop;

namespace YadaYada.CropperJs;

public class CropperInstance : IAsyncDisposable
{
    private readonly IJSObjectReference _jsInstance;
    private readonly DotNetObjectReference<Options> _objRef;
    private readonly IJSInProcessObjectReference _cropperWrapper;

    public CropperInstance(IJSObjectReference jsInstance, DotNetObjectReference<Options> objRef, IJSInProcessObjectReference cropperWrapper)
    {
        this._jsInstance = jsInstance;
        this._objRef = objRef;
        this._cropperWrapper = cropperWrapper;
    }
    public State State
    {
        get { return _cropperWrapper.Invoke<State>("getStateOfInstance", _jsInstance); }
    }
    public async Task ForceUpdate() => await _cropperWrapper.InvokeVoidAsync("forceUpdate");
    public async Task<State> Update() => await _cropperWrapper.InvokeAsync<State>("updateOnInstance", _jsInstance);
    public async Task<State> SetOptions(Options options) => await _cropperWrapper.InvokeAsync<State>("setOptionsOnInstance", _jsInstance, options, _objRef);
    public async Task Destroy() => await _jsInstance.InvokeVoidAsync("destroy");

    public async Task Zoom(decimal ratio) => await _cropperWrapper.InvokeVoidAsync("zoom", _jsInstance, ratio);
    public async Task DragMode(DragModeEnum mode)
    {
        const string none = "none";
        const string move = "move";
        const string crop = "crop";

        switch (mode)
        {
            case DragModeEnum.Cropper:
                await _cropperWrapper.InvokeVoidAsync("setDragMode", _jsInstance, crop);
                break;
            case DragModeEnum.Image:
                await _cropperWrapper.InvokeVoidAsync("setDragMode", _jsInstance, move);
                break;
            case DragModeEnum.None:
                await _cropperWrapper.InvokeVoidAsync("setDragMode", _jsInstance, none);
                break;
            default: 
                throw new ArgumentOutOfRangeException(nameof(mode));
        }
    }


    public async ValueTask DisposeAsync()
    {
        _objRef?.Dispose();
        if (_cropperWrapper != null) await _cropperWrapper.DisposeAsync();
    }
}

public enum DragModeEnum
{
    None = 1,
    Cropper,
    Image,
}
