using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract;
using WineHunterX.Model;
using WineHunterX.View.Util;
using Xamarians.CropImage;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XLabs.Ioc;
using XLabs.Platform.Device;
using XLabs.Platform.Services.Media;

namespace WineHunterX.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ConsultaVinhoOCR : ContentPage
	{
        private Button _takePictureButton;
        private Label _recognizedTextLabel;
        //private ProgressBar _progressBar;
        private Image _takenImage;

        private readonly ITesseractApi _tesseractApi;
        private readonly IDevice _device;

        public ConsultaVinhoOCR ()
		{
			InitializeComponent ();
            //On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    Padding = new Thickness(0, 25, 0, 0);
                    break;
                case Device.Android:
                    Padding = new Thickness(0, 0, 0, 0);
                    break;
                case Device.UWP:
                    Padding = new Thickness(0, 0, 0, 0);
                    break;
            }

            _tesseractApi = Resolver.Resolve<ITesseractApi>();
            _device = Resolver.Resolve<IDevice>();

            BuildUi();

            WireEvents();

            TakePictureButton_Clicked(this, new EventArgs());

        }

        private void BuildUi()
        {
            _takePictureButton = new Button
            {
                Text = "Clique para capturar a foto!"
            };

            _recognizedTextLabel = new Label();


            _takenImage = new Image() { HeightRequest = 200 };

            Content = new ScrollView
            {
                Content = new StackLayout
                {
                    Children =
                    {
                        _takePictureButton,
                        _takenImage,
                        _recognizedTextLabel
                    }
                }
            };

        }
        private void WireEvents()
        {
            _takePictureButton.Clicked += TakePictureButton_Clicked;
        }

        async void TakePictureButton_Clicked(object sender, EventArgs e)
        {

            _takePictureButton.Text = "Carregando...";
            _takePictureButton.IsEnabled = false;

            if (!_tesseractApi.Initialized)
                await _tesseractApi.Init("por");

            var photo = await TakePic();
            if (photo != null)
            {
                
                // When setting an ImageSource using a stream, 
                // the stream gets closed, so to avoid that I backed up
                // the image into a byte array with the following code:
                var imageBytes = new byte[photo.Source.Length];
                photo.Source.Position = 0;
                photo.Source.Read(imageBytes, 0, (int)photo.Source.Length);
                photo.Source.Position = 0;

                ////IEditableImage imagem;
                
                ////using (imagem = await CrossImageEdit.Current.CreateImageAsync(imageBytes))
                ////{
                ////    var croped = await Task.Run(() =>
                ////            imagem.Crop(10, 20, 250, 100)
                ////                 .Rotate(180)
                ////                 .Resize(100, 0)
                ////                 .ToPng()
                ////    );
                ////}

                var cropResult = await CropImageService.Instance.CropImage(photo.Path, CropRatioType.Square);
                //byte[] cropResult = await CrossXMethod.Current.CropImageFromOriginalToBytes(photo.Path);

                _takenImage.Source = cropResult.FilePath;

                //imageBytes = cropResult.FilePath;

                //var tessResult = await _tesseractApi.SetImage(imageBytes);
                var tessResult = await _tesseractApi.SetImage(cropResult.FilePath);

                if (tessResult)
                {
                    
                    ////_takenImage.Source = ImageSource.FromStream(() => photo.Source);
                    _recognizedTextLabel.Text = _tesseractApi.Text.Replace("&nbsp;", "")
                        .Trim();
                    //_recognizedTextLabel.Text = tessResult.ToString();
                    Termo termo = new Termo { Descricao = _recognizedTextLabel.Text };
                    await Navigation.PushAsync(new ListaVinho(termo));
                }
            }

            _takePictureButton.Text = "Nova Foto";
            _takePictureButton.IsEnabled = true;
        }

        private Xamarin.Forms.ImageSource GetImageSource(Stream m)
        {
            //FileStream path = Environment.GetFolderPath(Environment.SpecialFolder).ToString();


            //bm = BitmapFactory.DecodeFile(path);
            //MemoryStream stream = new MemoryStream();
            //bm.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
            //.Compress(System.Drawing.Bitmap.CompressFormat.Png, 100, stream);
            byte[] Base64Stream = Convert.FromBase64String(m.ToString());
            return ImageSource.FromStream(() => new MemoryStream(Base64Stream));
            //Xamarin.Forms.Image img = ImageSource.FromStream(stream.ToArray());
        }

        private async Task<MediaFile> TakePic()
        {
            var mediaStorageOptions = new CameraMediaStorageOptions
            {
                DefaultCamera = CameraDevice.Rear,
                Directory = "Pictures",
                Name = "OCRimage"
            };
            var mediaFile = await _device.MediaPicker.TakePhotoAsync(mediaStorageOptions);

            return mediaFile;
        }
    }
}