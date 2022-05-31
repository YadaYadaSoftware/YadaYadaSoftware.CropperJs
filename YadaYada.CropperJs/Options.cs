using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.JSInterop;

namespace YadaYada.CropperJs;

public class OptionsConverter : System.Text.Json.Serialization.JsonConverter<Options>
{
    public override Options? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, Options value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        switch (value.DragMode)
        {
            case DragModeEnum.Image:
                writer.WriteString("dragMode", "move");
                break;
            case DragModeEnum.Cropper:
                writer.WriteString("dragMode", "crop");
                break;
            case DragModeEnum.None:
                writer.WriteString("dragMode", "none");
                break;
            default:
                throw new NotSupportedException(value.DragMode.ToString());
        }
        writer.WriteEndObject();
    }
}

[JsonConverter(typeof(OptionsConverter))]
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

    [JsonPropertyName("dragMode")] public DragModeEnum DragMode { get; set; } = DragModeEnum.Cropper;

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