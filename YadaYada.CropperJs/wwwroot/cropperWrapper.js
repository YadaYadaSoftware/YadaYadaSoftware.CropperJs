function includeJs(jsFilePath) {
    var js = document.createElement("script");

    js.type = "text/javascript";
    js.src = jsFilePath;

    document.body.appendChild(js);
}




export function createCropper(reference, options, objRef) {
    includeJs("_content/YadaYada.CropperJs/cropper.js");
    return new Cropper(reference, null);
}