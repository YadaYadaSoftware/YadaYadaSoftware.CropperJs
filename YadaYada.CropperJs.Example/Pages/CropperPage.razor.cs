using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Components;
using YadaYada.CropperJs.Example.Annotations;

namespace YadaYada.CropperJs.Example.Pages
{
    public class CropModel : INotifyPropertyChanged
    {
        private int _x;
        private int _y;
        private int _width;
        private int _height;
        private decimal _zoomLevel;
        private decimal _rotation;



        public int X
        {
            get => _x;
            set
            {
                if (_x==value) return;
                _x = value;
                OnPropertyChanged();
            }
        }

        public int Y
        {
            get => _y;
            set
            {
                if(_y==value) return;
                _y = value;
                OnPropertyChanged();
            }
        }

        public int Width
        {
            get => _width;
            set
            {
                if (value == _width) return;
                _width = value;
                OnPropertyChanged();
            }
        }

        public int Height
        {
            get => _height;
            set
            {
                if (value == _height) return;
                _height = value;
                OnPropertyChanged();
            }
        }

        public decimal ZoomLevel
        {
            get => _zoomLevel;
            set
            {
                if (value == _zoomLevel) return;
                _zoomLevel = value;
                OnPropertyChanged();
            }
        }

        public decimal Rotation

        {
            get => _rotation;
            set
            {
                if (value == _rotation) return;
                _rotation = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public partial class CropperPage
    {
        private Cropper _cropper = null!;
        private decimal _zoom = 1;
        private string _log = string.Empty;
        private decimal _rotation;
        private DragModeEnum _dragMode = DragModeEnum.Image;
        private CropModel Model { get; set; } = new CropModel() { X = 10, Y = 10, Width = 10, Height = 10};

        protected override Task OnInitializedAsync()
        {
            this.Model.PropertyChanged += Model_PropertyChanged;
            return base.OnInitializedAsync();
        }

        private void Model_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            this.Log += e.PropertyName + "->" + this.Model.GetType().GetProperty(e.PropertyName).GetValue(this.Model) + Environment.NewLine;
        }

        public decimal Zoom
        {
            get => _zoom;
            private set
            {
                if (_zoom == value) return;
                _zoom = value;
                this.StateHasChanged();
            }
        }

        public EventCallback<string> LogChanged { get; set; }
        public string Log
        {
            get => _log;
            set
            {
                if (_log==value) return;
                _log = value;
                LogChanged.InvokeAsync(value);
            }
        }

        public decimal Rotation
        {
            get => _rotation;
            set
            {
                if (_rotation==value) return;
                _log += nameof(Rotation) + ':' + _rotation + "=>" + value + Environment.NewLine;
                LogChanged.InvokeAsync(_log);
                _rotation = value;
            }
        }

        public DragModeEnum DragMode
        {
            get => _dragMode;
            set
            {
                if (_dragMode == value) return;
                _dragMode = value;
            }
        }


        private Task ZoomTo(decimal ratio)
        {
            return _cropper.ZoomTo(ratio);
        }

        private Task Rotate(int degrees)
        {
            return _cropper.Rotate(degrees);
        }

        private Task RotateTo(int i)
        {
            return _cropper.RotateTo(i);
        }
    }
}
