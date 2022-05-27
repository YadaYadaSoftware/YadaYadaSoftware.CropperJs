using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Logging;

namespace YadaYada.CropperJs
{
    public partial class Cropper
    {
        private ElementReference _image;
        [Inject] private CropperInterop CropperInterop { get; set; } = null!;

        [Inject] private ILogger<Cropper> Logger { get; set; } = null!;

        [Parameter]
        public string ImageSource { get; set; } = null!;

        private async Task ImageLoaded(ProgressEventArgs arg)
        {
            Logger.LogInformation(nameof(ImageLoaded));
            await CropperInterop.CreatePopperAsync(_image, new Options());
        }
    }
}
