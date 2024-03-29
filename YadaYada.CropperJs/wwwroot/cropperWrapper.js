﻿export function createCropper(reference, objRef, externalOptions) {

    'use strict';

    var Cropper = window.Cropper;
    var URL = window.URL || window.webkitURL;
    var image = reference;
    var download = document.getElementById('download');
    var actions = document.getElementById('actions');
    var dataX = document.getElementById('dataX');
    var dataY = document.getElementById('dataY');
    var dataHeight = document.getElementById('dataHeight');
    var dataWidth = document.getElementById('dataWidth');
    var dataRotate = document.getElementById('dataRotate');
    var dataScaleX = document.getElementById('dataScaleX');
    var dataScaleY = document.getElementById('dataScaleY');
    var options = {
        aspectRatio: NaN,
        dragMode: externalOptions.dragMode,
        data: externalOptions.data,
        preview: '.img-preview',
        ready: function (e) {
            console.log(e.type);
            objRef.invokeMethodAsync('ready');
        },
        cropstart: function (e) {
            console.log(e.type, e.detail.action);
        },
        cropmove: function (e) {
            console.log(e.type, e.detail.action);
        },
        //cropend: function (e) {
        //    console.log(e.type, e.detail.action);
        //    var cropData = cropper.getData();
        //    objRef.invokeMethodAsync('cropend', cropData);
        //},
        crop: function (e) {
            var data = e.detail;
            console.log(e.type);
            if (dataX) dataX.value = Math.round(data.x);
            if (dataY) dataY.value = Math.round(data.y);
            if (dataHeight) dataHeight.value = Math.round(data.height);
            if (dataWidth) dataWidth.value = Math.round(data.width);
            if (dataRotate) dataRotate.value = typeof data.rotate !== 'undefined' ? data.rotate : '';
            if (dataScaleX) dataScaleX.value = typeof data.scaleX !== 'undefined' ? data.scaleX : '';
            if (dataScaleY) dataScaleY.value = typeof data.scaleY !== 'undefined' ? data.scaleY : '';
            objRef.invokeMethodAsync('crop', data );
        },
        zoom: function (e) {
            console.log(e.type, e.detail.ratio);
            objRef.invokeMethodAsync('zoom', e.detail.ratio);
        }
    };


    if (!externalOptions.cropEnabled) {
        options.autoCrop = false;
        //options.dragMode = 'none';
    }
    var cropper = new Cropper(image, options);
    var originalImageURL = image.src;
    var uploadedImageType = 'image/jpeg';
    var uploadedImageName = 'cropped.jpg';
    var uploadedImageURL;

    // Tooltip
    //$('[data-toggle="tooltip"]').tooltip();

    // Buttons
    if (!document.createElement('canvas').getContext) {
        $('button[data-method="getCroppedCanvas"]').prop('disabled', true);
    }

    if (typeof document.createElement('cropper').style.transition === 'undefined') {
        $('button[data-method="rotate"]').prop('disabled', true);
        $('button[data-method="scale"]').prop('disabled', true);
    }

    // Download
    if (download != null) {
        if (typeof download.download === 'undefined') {
            download.className += ' disabled';
            download.title = 'Your browser does not support download';
            }
    }

    if (actions !== null) {
            // Options
        var toggles = actions.querySelector('.docs-toggles');

        if (toggles !== null) {
            toggles.onchange = function (event) {
                var e = event || window.event;
                var target = e.target || e.srcElement;
                var cropBoxData;
                var canvasData;
                var isCheckbox;
                var isRadio;

                if (!cropper) {
                    return;
                }

                if (target.tagName.toLowerCase() === 'label') {
                    target = target.querySelector('input');
                }

                isCheckbox = target.type === 'checkbox';
                isRadio = target.type === 'radio';

                if (isCheckbox || isRadio) {
                    if (isCheckbox) {
                        options[target.name] = target.checked;
                        cropBoxData = cropper.getCropBoxData();
                        canvasData = cropper.getCanvasData();

                        options.ready = function () {
                            console.log('ready');
                            cropper.setCropBoxData(cropBoxData).setCanvasData(canvasData);
                        };
                    } else {
                        options[target.name] = target.value;
                        options.ready = function () {
                            console.log('ready');
                        };
                    }

                    // Restart
                    cropper.destroy();
                    cropper = new Cropper(image, options);
                }
            };

        }

        // Methods
        var buttons = actions.querySelector('.docs-buttons');
        if (buttons !== null) {
            actions.querySelector('.docs-buttons').onclick = function (event) {
                var e = event || window.event;
                var target = e.target || e.srcElement;
                var cropped;
                var result;
                var input;
                var data;

                if (!cropper) {
                    return;
                }

                while (target !== this) {
                    if (target.getAttribute('data-method')) {
                        break;
                    }

                    target = target.parentNode;
                }

                if (target === this || target.disabled || target.className.indexOf('disabled') > -1) {
                    return;
                }

                data = {
                    method: target.getAttribute('data-method'),
                    target: target.getAttribute('data-target'),
                    option: target.getAttribute('data-option') || undefined,
                    secondOption: target.getAttribute('data-second-option') || undefined
                };

                cropped = cropper.cropped;

                if (data.method) {
                    if (typeof data.target !== 'undefined') {
                        input = document.querySelector(data.target);

                        if (!target.hasAttribute('data-option') && data.target && input) {
                            try {
                                data.option = JSON.parse(input.value);
                            } catch (e) {
                                console.log(e.message);
                            }
                        }
                    }

                    switch (data.method) {
                        case 'rotate':
                            if (cropped && options.viewMode > 0) {
                                cropper.clear();
                            }

                            break;

                        case 'getCroppedCanvas':
                            try {
                                data.option = JSON.parse(data.option);
                            } catch (e) {
                                console.log(e.message);
                            }

                            if (uploadedImageType === 'image/jpeg') {
                                if (!data.option) {
                                    data.option = {};
                                }

                                data.option.fillColor = '#fff';
                            }

                            break;
                    }

                    result = cropper[data.method](data.option, data.secondOption);

                    switch (data.method) {
                        case 'rotate':
                            if (cropped && options.viewMode > 0) {
                                cropper.crop();
                            }

                            break;

                        case 'scaleX':
                        case 'scaleY':
                            target.setAttribute('data-option', -data.option);
                            break;

                        case 'getCroppedCanvas':
                            if (result) {
                                // Bootstrap's Modal
                                $('#getCroppedCanvasModal').modal().find('.modal-body').html(result);

                                if (!download.disabled) {
                                    download.download = uploadedImageName;
                                    download.href = result.toDataURL(uploadedImageType);
                                }
                            }

                            break;

                        case 'destroy':
                            cropper = null;

                            if (uploadedImageURL) {
                                URL.revokeObjectURL(uploadedImageURL);
                                uploadedImageURL = '';
                                image.src = originalImageURL;
                            }

                            break;
                    }

                    if (typeof result === 'object' && result !== cropper && input) {
                        try {
                            input.value = JSON.stringify(result);
                        } catch (e) {
                            console.log(e.message);
                        }
                    }
                }
            };
        }

    }
    document.body.onkeydown = function (event) {
        var e = event || window.event;

        if (e.target !== this || !cropper || this.scrollTop > 300) {
            return;
        }

        switch (e.keyCode) {
            case 37:
                e.preventDefault();
                cropper.move(-1, 0);
                break;

            case 38:
                e.preventDefault();
                cropper.move(0, -1);
                break;

            case 39:
                e.preventDefault();
                cropper.move(1, 0);
                break;

            case 40:
                e.preventDefault();
                cropper.move(0, 1);
                break;
        }
    };

    // Import image
    var inputImage = document.getElementById('inputImage');

    if (inputImage!==null) {
        if (URL) {
            inputImage.onchange = function () {
                var files = this.files;
                var file;

                if (files && files.length) {
                    file = files[0];

                    if (/^image\/\w+/.test(file.type)) {
                        uploadedImageType = file.type;
                        uploadedImageName = file.name;

                        if (uploadedImageURL) {
                            URL.revokeObjectURL(uploadedImageURL);
                        }

                        image.src = uploadedImageURL = URL.createObjectURL(file);

                        if (cropper) {
                            cropper.destroy();
                        }

                        cropper = new Cropper(image, options);
                        inputImage.value = null;
                    } else {
                        window.alert('Please choose an image file.');
                    }
                }
            };
        } else {
            inputImage.disabled = true;
            inputImage.parentNode.className += ' disabled';
        }

    }

    return cropper;

}

export function zoom(instance, scale) {
    instance.zoom(scale, null);
}
export function zoomTo(instance, scale) {
    instance.zoomTo(scale, null);
}

export function rotate(instance, degrees) {
    instance.rotate(degrees, null);
}

export function rotateTo(instance, degrees) {
    instance.rotateTo(degrees, null);
}

export function setDragMode(instance, mode) {
    instance.setDragMode(mode);
}
export function setData(instance,data) {
    instance.setData(data);
}

