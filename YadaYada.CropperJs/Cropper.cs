using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Logging;

namespace YadaYadaSoftware.CropperJs;

public partial class Cropper : IDisposable
{
    private ElementReference _image;
    private bool _ready;
    private CropperInstance _cropperInstance = null!;
    private decimal? _zoomLevel = null;

    [Inject] private ILogger<Cropper> Logger { get; set; } = null!;
    [Inject] private CropperFactory CropperFactory { get; set; } = null!;

    [Parameter]
    public string ImageSource { get; set; } = null!;

    [Parameter]
    public bool ShowTextBoxes
    {
        get => _showTextBoxes;
        set
        {
            if (_showTextBoxes == value) return;
            _showTextBoxes = value;
            this.StateHasChanged();
            ShowTextBoxesChanged.InvokeAsync(value);
        }
    }

    [Parameter]
    public EventCallback<bool> ShowTextBoxesChanged { get; set; }


    [Parameter]
    public decimal ZoomLevel
    {
        get => _zoomLevel ?? 1;
        set
        {
            if (_zoomLevel == value) return;
            _zoomLevel = value;
            ZoomLevelChanged.InvokeAsync(value);
                
        }
    }

    [Parameter]
    public EventCallback<decimal> ZoomLevelChanged { get; set; }


    private async Task ImageLoaded(ProgressEventArgs arg)
    {
        Logger.LogInformation(nameof(ImageLoaded));
        // ReSharper disable once ConditionIsAlwaysTrueOrFalse
        if (_cropperInstance == null)
        {
            var options = new Options
            {
                OnCrop = CropHandler,
                OnZoom = ZoomHandler,
                OnReady = ReadyHandler,
                Data = new CropData { X = this.CropX, Y = this.CropY, Width = this.CropWidth, Height = this.CropHeight, Rotation = this.Rotation },
                CropEnabled = this.CropEnabled
            };
            switch (this.DragMode)
            {
                case DragModeEnum.Image:
                    options.DragMode = "move";
                    break;
                case DragModeEnum.Cropper:
                    options.DragMode = "crop";
                    break;
                case DragModeEnum.None:
                    options.DragMode = "none";
                    break;
                default:
                    throw new NotSupportedException(this.DragMode.ToString());

            }
            _cropperInstance = await CropperFactory.CreateCropperAsync(_image, options);
        }
    }

    private async void ReadyHandler()
    {
        _ready = true;
        if (_zoomLevel.HasValue) await _cropperInstance.ZoomTo(_zoomLevel.Value);
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
    private DragModeEnum _dragMode = DragModeEnum.Cropper;
    private bool _showNativeCropperJsToolbar = false;
    private bool _showTextBoxes = false;
    private bool _showCropInfo;
    private bool _cropEnabled = true;
    private bool _showZoomLevel;

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

    public Task Zoom(decimal i)
    {
        return _cropperInstance.Zoom(i);
    }

    public Task ZoomTo(decimal zoomLevel)
    {
        return _cropperInstance.ZoomTo(zoomLevel);
    }
    public Task RotateTo(decimal degrees)
    {
        return _cropperInstance.RotateTo(degrees);
    }
    public Task Rotate(decimal degrees)
    {
        return _cropperInstance.Rotate(degrees);
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

    [Parameter] public EventCallback<bool> ShowNativeCropperJsToolbarChanged { get; set; }

    [Parameter]
    public bool ShowNativeCropperJsToolbar
    {
        get => _showNativeCropperJsToolbar;
        set
        {
            if(_showNativeCropperJsToolbar==value) return;
            _showNativeCropperJsToolbar = value;
            this.StateHasChanged();
            ShowNativeCropperJsToolbarChanged.InvokeAsync(value);

        }
    }

    [Parameter] public RenderFragment ChildContent { get; set; } = null!;

    private bool ShowStatusBar => this.ShowCropInfo || this.ShowZoomLevel;

    [Parameter]
    public bool ShowZoomLevel
    {
        get => _showZoomLevel;
        set
        {
            if (_showZoomLevel==value) return;
            _showZoomLevel = value;
            ShowZoomLevelChanged.InvokeAsync(value);
        }
    }

    [Parameter]
    public EventCallback<bool> ShowZoomLevelChanged { get; set; }

    [Parameter]
    public bool ShowCropInfo
    {
        get => _showCropInfo;
        set
        {
            if (_showCropInfo==value) return;
            _showCropInfo = value;
            ShowStatusBarChanged.InvokeAsync(value);
        }
    }

    [Parameter] public EventCallback<bool> ShowStatusBarChanged { get; set; }

    public void Dispose()
    {
        _cropperInstance?.Dispose();
    }

    [Parameter]
    public bool CropEnabled
    {
        get => _cropEnabled;
        set => _cropEnabled = value;
    }
}