using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tesseract;
using Xamarin.Forms;
using XLabs.Ioc;
using XLabs.Platform.Device;
using XLabs.Platform.Services.Media;

namespace WineHunterX.View
{
    public partial class HomePage : ContentPage
    {
        private Button _takePictureButton;
        private Label _recognizedTextLabel;
        private Image _takenImage;

        private readonly ITesseractApi _tesseractApi;
        private readonly IDevice _device;

        public HomePage()
        {
            InitializeComponent();
            _tesseractApi = Resolver.Resolve<ITesseractApi>();
            _device = Resolver.Resolve<IDevice>();

            _takenImage = new Image()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Color.Gray
            };

            _takePictureButton = new Button()
            {
                Text = "Clique Aqui Para Tirar Foto de um Novo Rótulo",
                TextColor = Color.White,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Center,
                BackgroundColor = Color.MediumVioletRed
            };

            _recognizedTextLabel = new Label();

            StackLayout stack = new StackLayout()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Children = { _takePictureButton, _takenImage, _recognizedTextLabel },
                BackgroundColor = Color.MediumVioletRed
            };

            ScrollView scroll = new ScrollView()
            {
                Content = stack
            };

            Content = scroll;

            _takePictureButton.Clicked += TakePictureButton_Clicked;
        }

        async void TakePictureButton_Clicked(object sender, EventArgs e)
        {
            if (!_tesseractApi.Initialized)
                //Tesseract *tesseract = [[Tesseract alloc] initWithLanguage:@"por"];
                await _tesseractApi.Init("por");

            var photo = await TakePic();
            if (photo != null)
            {
                var imageBytes = new byte[photo.Source.Length];
                photo.Source.Position = 0;
                photo.Source.Read(imageBytes, 0, (int)photo.Source.Length);
                photo.Source.Position = 0;

                _takenImage.Source = ImageSource.FromStream(() => photo.Source);
                var tessResult = await _tesseractApi.SetImage(imageBytes);
                if (tessResult)
                {
                    _recognizedTextLabel.Text = _tesseractApi.Text;
                }
            }
        }

        private async Task<MediaFile> TakePic()
        {
            var mediaStorageOptions = new CameraMediaStorageOptions
            {
                DefaultCamera = CameraDevice.Rear
            };
            var mediaFile = await _device.MediaPicker.TakePhotoAsync(mediaStorageOptions);

            return mediaFile;
        }
    }
}
