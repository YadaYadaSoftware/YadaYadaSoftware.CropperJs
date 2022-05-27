using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace YadaYada.CropperJs
{
    public partial class Cropper
    {
        [Parameter]
        public string ImageSource { get; set; }
    }
}
