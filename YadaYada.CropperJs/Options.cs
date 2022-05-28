using System.Text.Json.Serialization;
using Microsoft.JSInterop;

namespace YadaYada.CropperJs;

public class Options
{
    [JsonIgnore]
    public Action<CropEnd> OnCropEnd { get; set; }

    [JSInvokable("cropend")]
    public void CallOnCropEnd(CropEnd cropEnd) => OnCropEnd?.Invoke(cropEnd);

    [JsonIgnore]
    public Action<CropEventArgs> OnCrop { get; set; }

    [JSInvokable("crop")]
    public void Crop(CropEventArgs crop) => OnCrop?.Invoke(crop);

    [JsonIgnore]
    public Action<decimal> OnZoom { get; set; }

    [JSInvokable("zoom")]
    public void Zoom(decimal ratio) => OnZoom?.Invoke(ratio);

    [JsonIgnore]
    public Action OnReady { get; set; } = null!;

    [JSInvokable("ready")]
    public void Ready() => OnReady.Invoke();


}

public class CropEnd
{
    [JsonPropertyName("x")]
    public decimal X { get; set; }

}

public class CropEventArgs : EventArgs
{
    [JsonPropertyName("x")] public decimal X { get; set; }
    [JsonPropertyName("y")] public decimal Y { get; set; }
    [JsonPropertyName("width")] public decimal Width { get; set; }
    [JsonPropertyName("height")] public decimal Height { get; set; }
    [JsonPropertyName("rotate")] public decimal Rotation { get; set; }
}

public class CropData
{
    [JsonPropertyName("x")] public decimal X { get; set; }
    [JsonPropertyName("y")] public decimal Y { get; set; }
    [JsonPropertyName("width")] public decimal Width { get; set; }
    [JsonPropertyName("height")] public decimal Height { get; set; }
    [JsonPropertyName("rotate")] public decimal Rotation { get; set; }
}