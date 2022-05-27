window.cropper = null;

export function createCropper(reference, options) {

    if (window.cropper) {
        return window.cropper;
    }

    var options = {
        aspectRatio: 16 / 9,
        preview: '.img-preview',
        ready: function (e) {
            console.log(e.type);
            helpers.invokeMethodAsync("GetCrop").then(data => {
                console.log(data);
                var x = { x: data.x, y: data.y, width: data.width, height: data.height };
                cropper.setData(x);

            });
        },
        cropstart: function (e) {
            console.log(e.type, e.detail.action);
        },
        cropmove: function (e) {
            console.log(e.type, e.detail.action);
        },
        cropend: function (e) {
            console.log(e.type, e.detail);
            var cropData = cropper.getData();
            helpers.invokeMethodAsync("CropEnd", cropData.x, cropData.y, cropData.width, cropData.height);
        },
        crop: function (e) {
            var data = e.detail;
            console.log(e.type);
            dataX.value = Math.round(data.x);
            dataY.value = Math.round(data.y);
            dataHeight.value = Math.round(data.height);
            dataWidth.value = Math.round(data.width);
            dataRotate.value = typeof data.rotate !== 'undefined' ? data.rotate : '';
            dataScaleX.value = typeof data.scaleX !== 'undefined' ? data.scaleX : '';
            dataScaleY.value = typeof data.scaleY !== 'undefined' ? data.scaleY : '';
        },
        zoom: function (e) {
            console.log(e.type, e.detail.ratio);
            helpers.invokeMethodAsync("Zoom", e.detail.ratio);


        }
    };

    if (!window.cropper) {
        window.cropper = new Cropper(reference, options);
    }
    return window.cropper;
}