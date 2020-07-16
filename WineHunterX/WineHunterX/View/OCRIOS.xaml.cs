using System;
using System.Threading.Tasks;
using XLabs.Ioc;
using XLabs.Platform.Services.Media;
using Xamarin.Forms;
using Tesseract;

namespace WineHunterX.View
{
    public partial class OCRIOS : ContentPage
    {
        private readonly IMediaPicker _mediaPicker;
        private readonly ITesseractApi _tesseract;

        public OCRIOS()
        {
            InitializeComponent();
            _mediaPicker = Resolver.Resolve<IMediaPicker>();
            _tesseract = Resolver.Resolve<ITesseractApi>();
        }

        private async void LoadImageButton_OnClicked(object sender, EventArgs e)
        {
            var result = await _mediaPicker.SelectPhotoAsync(new CameraMediaStorageOptions());
            await Recognise(result);
        }

        private async void GetPhotoButton_OnClicked(object sender, EventArgs e)
        {
            var result = await _mediaPicker.TakePhotoAsync(new CameraMediaStorageOptions());
            await Recognise(result);
        }

        public async Task Recognise(MediaFile result)
        {
            if (result.Source == null)
                return;
            activityIndicator.IsRunning = true;

            //_tesseract.setDatapath("/tessdata");

            if (!_tesseract.Initialized)
            {
                var initialised = await _tesseract.Init("por");
                if (!initialised)
                    return;
            }
            await _tesseract.SetImage(result.Source)
                .ContinueWith((t) =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        activityIndicator.IsRunning = false;
                        TextLabel.Text = _tesseract.Text;
                        //var words = _tesseract.Results(PageIteratorLevel.Word);
                        //var symbols = _tesseract.Results(PageIteratorLevel.Symbol);
                        //var blocks = _tesseract.Results(PageIteratorLevel.Block);
                        //var paragraphs = _tesseract.Results(PageIteratorLevel.Paragraph);
                        //var lines = _tesseract.Results(PageIteratorLevel.Textline);
                    });
                });

        }
    }
}
