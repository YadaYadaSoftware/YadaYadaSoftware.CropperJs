using System.Text.Json.Serialization;

namespace YadaYada.CropperJs;

public class CropData
{
    [JsonPropertyName("x")] public decimal X { get; set; }
    [JsonPropertyName("y")] public decimal Y { get; set; }
    [JsonPropertyName("width")] public decimal Width { get; set; }
    [JsonPropertyName("height")] public decimal Height { get; set; }
    [JsonPropertyName("rotate")] public decimal Rotation { get; set; }
}