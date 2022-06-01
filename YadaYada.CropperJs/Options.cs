using System.Text.Json.Serialization;
using Microsoft.JSInterop;

namespace YadaYada.CropperJs;

//[JsonConverter(typeof(OptionsConverter))]
public class Options
{
    [JsonIgnore]
    public Action<CropEventArgs> OnCrop { get; set; }

    [JsonIgnore]
    public Action<decimal> OnZoom { get; set; }

    [JsonIgnore]
    public Action OnReady { get; set; } = null!;

    [JSInvokable("ready")]
    public void Ready() => OnReady.Invoke();

    [JSInvokable("crop")]
    public void Crop(CropEventArgs crop) => OnCrop?.Invoke(crop);

    [JSInvokable("zoom")]
    public void Zoom(decimal ratio) => OnZoom?.Invoke(ratio);


    [JsonPropertyName("dragMode")] public string DragMode { get; set; } = "crop";
    [JsonPropertyName("data")] public CropData Data { get; set; }
    [JsonPropertyName("cropEnabled")] public bool CropEnabled { get; set; }



}