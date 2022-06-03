
In your `Program.cs`, make sure to:

    builder.Services.AddCropperServices();

In your `index.html`, make sure to add in your <head> section:

    <link href="./_content/YadaYadaSoftware.CropperJs/main-cropper.css" rel="stylesheet" />
    <link href="./_content/YadaYadaSoftware.CropperJs/cropper.css" rel="stylesheet" />
    <link href="./_content/YadaYadaSoftware.CropperJs/bootstrap.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.1.1/css/all.min.css" integrity="sha512-KfkfwYDsLkIlwQp6LFnl8zNdLGxu9YAA1QvwINks4PhcElQSvqcyVLLD9aMhXd13uQjoXtEKNosOWaZqXgel0g==" crossorigin="anonymous" referrerpolicy="no-referrer" />

example:

	<head>
	    <meta charset="utf-8" />
	    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />
	    <base href="/" />

	    <!-- Begin YadaYadaSoftware.CropperJs -->
	    <link href="./_content/YadaYadaSoftware.CropperJs/main-cropper.css" rel="stylesheet" />
	    <link href="./_content/YadaYadaSoftware.CropperJs/cropper.css" rel="stylesheet" />
	    <link href="./_content/YadaYadaSoftware.CropperJs/bootstrap.css" rel="stylesheet" />
	    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.1.1/css/all.min.css" integrity="sha512-KfkfwYDsLkIlwQp6LFnl8zNdLGxu9YAA1QvwINks4PhcElQSvqcyVLLD9aMhXd13uQjoXtEKNosOWaZqXgel0g==" crossorigin="anonymous" referrerpolicy="no-referrer" />
	    <!-- End YadaYadaSoftware.CropperJs -->
	</head>

In your `index.html`, make sure to add in your <body> section:

    <script src="./_content/YadaYadaSoftware.CropperJs/cropper.js"></script>
    <script src="./_content/YadaYadaSoftware.CropperJs/bootstrap.js"></script>
    <script src="./_content/YadaYadaSoftware.CropperJs/jquery-3.6.0.min.js"></script>

Example:

	<body>
	    <div id="app">Loading...</div>

	    <div id="blazor-error-ui">
	        An unhandled error has occurred.
	        <a href="" class="reload">Reload</a>
	        <a class="dismiss">🗙</a>
	    </div>
	    <script src="_framework/blazor.webassembly.js"></script>
	    <script src="./_content/YadaYadaSoftware.CropperJs/cropper.js"></script>
	    <script src="./_content/YadaYadaSoftware.CropperJs/bootstrap.js"></script>
	    <script src="./_content/YadaYadaSoftware.CropperJs/jquery-3.6.0.min.js"></script>

	</body>

