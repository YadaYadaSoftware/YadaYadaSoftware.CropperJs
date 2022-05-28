using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Logging;

namespace YadaYada.CropperJs
{
    public partial class Cropper
    {
        private ElementReference _image;

        [Inject] private ILogger<Cropper> Logger { get; set; } = null!;
        [Inject] private CropperFactory CropperFactory { get; set; } = null!;

        [Parameter]
        public string ImageSource { get; set; } = null!;

        [Parameter]
        public decimal Zoom
        {
            get => _zoom;
            set
            {
                if (_zoom == value) return;
                _zoom = value;
                ZoomChanged.InvokeAsync(value);
                
            }
        }

        [Parameter]
        public EventCallback<decimal> ZoomChanged { get; set; }

        private CropperInstance _cropperInstance = null!;
        private decimal _zoom;

        private async Task ImageLoaded(ProgressEventArgs arg)
        {
            Logger.LogInformation(nameof(ImageLoaded));
            if (_cropperInstance == null)
            {
                var options = new Options();
                options.OnCropEnd = CropEndHandler;
                options.OnCrop = CropHandler;
                options.OnZoom = ZoomHandler;
                _cropperInstance = await CropperFactory.CreateCropperAsync(_image, options);
            }
        }

        private void ZoomHandler(decimal ratio)
        {
            this.Zoom = ratio;
        }

        #region CropX
        private decimal _cropX;

        [Parameter]
        public decimal CropX
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
        public EventCallback<decimal> CropXChanged { get; set; }

        #endregion

        #region CropY
        private decimal _cropY;

        [Parameter]
        public decimal CropY
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
        public EventCallback<decimal> CropYChanged { get; set; }

        #endregion

        #region CropWidth
        private decimal _cropWidth;

        [Parameter]
        public decimal CropWidth
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
        public EventCallback<decimal> CropWidthChanged { get; set; }

        #endregion

        #region CropHeight
        private decimal _cropHeight;

        [Parameter]
        public decimal CropHeight
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
        public EventCallback<decimal> CropHeightChanged { get; set; }

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
            Logger.LogInformation(crop.X.ToString() + ',' + crop.Y + ',' + crop.Width + ',' + crop.Height + ',' + crop.Rotation);
            this.CropX = crop.X;
            this.CropY = crop.Y;
            this.CropWidth = crop.Width;
            this.CropHeight = crop.Height;
            this.Rotation = crop.Rotation;
        }

        private void CropEndHandler(CropEnd obj)
        {
            Logger.LogInformation(nameof(Cropper) + "." + nameof(CropEndHandler));
        }

        public Task ZoomTo(decimal i)
        {
            this.Zoom = i;
            return _cropperInstance.Zoom(i);
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
    }
}
