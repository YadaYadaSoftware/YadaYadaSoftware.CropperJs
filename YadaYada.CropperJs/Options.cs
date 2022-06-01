using System.Text.Json.Serialization;
using Microsoft.JSInterop;

namespace YadaYada.CropperJs;

//[JsonConverter(typeof(OptionsConverter))]
public class Options
{
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

    [JsonPropertyName("dragMode")] public string DragMode { get; set; } = "crop";
    [JsonPropertyName("data")] public CropData Data { get; set; }
    [JsonPropertyName("cropEnabled")] public bool CropEnabled { get; set; }

    [JSInvokable("ready")]
    public void Ready() => OnReady.Invoke();


}