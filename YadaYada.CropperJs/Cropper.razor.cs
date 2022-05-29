using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace YadaYada.CropperJs;

public static class CropperServices
{
    public static IServiceCollection AddCropperServices(this IServiceCollection services)
    {
        services.TryAddSingleton<CropperFactory>();
        return services;
    }
}

public partial class Cropper
{
    private ElementReference _image;
    private bool _ready;

    [Inject] private ILogger<Cropper> Logger { get; set; } = null!;
    [Inject] private CropperFactory CropperFactory { get; set; } = null!;

    [Parameter]
    public string ImageSource { get; set; } = null!;

    [Parameter]
    public decimal ZoomLevel
    {
        get => _zoomLevel;
        set
        {
            if (_zoomLevel == value) return;
            _zoomLevel = value;
            ZoomLevelChanged.InvokeAsync(value);
                
        }
    }

    [Parameter]
    public EventCallback<decimal> ZoomLevelChanged { get; set; }

    private CropperInstance _cropperInstance = null!;
    private decimal _zoomLevel;

    private async Task ImageLoaded(ProgressEventArgs arg)
    {
        Logger.LogInformation(nameof(ImageLoaded));
        // ReSharper disable once ConditionIsAlwaysTrueOrFalse
        if (_cropperInstance == null)
        {
            var options = new Options();
            options.OnCropEnd = CropEndHandler;
            options.OnCrop = CropHandler;
            options.OnZoom = ZoomHandler;
            options.OnReady = ReadyHandler;
            _cropperInstance = await CropperFactory.CreateCropperAsync(_image, options);
        }
    }

    private async void ReadyHandler()
    {
        _ready = true;
        await _cropperInstance.SetCropAsync(new CropData { X = this.CropX, Y = this.CropY, Width = this.CropWidth, Height = this.CropHeight, Rotation = this.Rotation });
    }


    private void ZoomHandler(decimal ratio)
    {
        this.ZoomLevel = ratio;
    }

    #region CropX
    private int _cropX;

    [Parameter]
    public int CropX
    {
        get => _cropX;
        set
        {
            if (_cropX == value) return;
            _cropX = value;
            CropXChanged.InvokeAsync(value);
        }
    }

    [Parameter]
    public EventCallback<int> CropXChanged { get; set; }

    #endregion

    #region CropY
    private int _cropY;

    [Parameter]
    public int CropY
    {
        get => _cropY;
        set
        {
            if (_cropY == value) return;
            _cropY = value;
            CropYChanged.InvokeAsync(value);
        }
    }

    [Parameter]
    public EventCallback<int> CropYChanged { get; set; }

    #endregion

    #region CropWidth
    private int _cropWidth;

    [Parameter]
    public int CropWidth
    {
        get => _cropWidth;
        set
        {
            if (_cropWidth == value) return;
            _cropWidth = value;
            CropWidthChanged.InvokeAsync(value);
        }
    }

    [Parameter]
    public EventCallback<int> CropWidthChanged { get; set; }

    #endregion

    #region CropHeight
    private int _cropHeight;

    [Parameter]
    public int CropHeight
    {
        get => _cropHeight;
        set
        {
            if (_cropHeight == value) return;
            _cropHeight = value;
            CropHeightChanged.InvokeAsync(value);
        }
    }

    [Parameter]
    public EventCallback<int> CropHeightChanged { get; set; }

    #endregion
    private decimal _rotation;
    private DragModeEnum _dragMode;

    [Parameter]
    public decimal Rotation
    {
        get => _rotation;
        set
        {
            if (_rotation == value) return;
            _rotation = value;
            RotationChanged.InvokeAsync(value);
        }
    }

    [Parameter]
    public EventCallback<decimal> RotationChanged { get; set; }

    private void CropHandler(CropEventArgs crop)
    {
        if (!_ready) return;
        Logger.LogInformation(crop.X.ToString() + ',' + crop.Y + ',' + crop.Width + ',' + crop.Height + ',' + crop.Rotation);
        this.CropX = (int) crop.X;
        this.CropY = (int) crop.Y;
        this.CropWidth = (int) crop.Width;
        this.CropHeight = (int) crop.Height;
        this.Rotation = crop.Rotation;
    }

    private void CropEndHandler(CropEnd obj)
    {
        Logger.LogInformation(nameof(Cropper) + "." + nameof(CropEndHandler));
    }

    public Task Zoom(decimal i)
    {
        return _cropperInstance.Zoom(i);
    }

    public ValueTask ZoomTo(decimal zoomLevel)
    {
        return _cropperInstance.ZoomTo(zoomLevel);
    }

    //CropEventArgs
    /*
        {"x":127.99999999999996,"y":71.99999999999997,"width":1024.0000000000002,"height":576.0000000000001,"rotate":0,"scaleX":1,"scaleY":1}
     */
    public Task SetCrop(decimal x, decimal y, decimal width, decimal height, decimal rotation)
    {
        var data = new CropData {X = x, Y = y, Width = width, Height = height, Rotation = rotation};
        return _cropperInstance.SetCropAsync(data);
    }

    [Parameter] public EventCallback<DragModeEnum> DragModeChanged { get; set; }

    [Parameter]
    public DragModeEnum DragMode
    {
        get => _dragMode;
        set
        {
            if(_dragMode==value) return;
            _dragMode = value;
            InvokeAsync(async () => { await _cropperInstance.DragMode(value); });
        }
    }

    [Parameter]
    public string Id { get; set; }

    private string ImageId => this.Id + "Image";

    [Parameter] public bool ShowNativeCropperJsToolbar { get; set; } = false;
    [Parameter] public RenderFragment ChildContent { get; set; } = null!;
}