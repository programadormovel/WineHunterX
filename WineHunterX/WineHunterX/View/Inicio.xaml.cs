using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tesseract;
using WineHunterX.Model;
using WineHunterX.View;
using Xamarians.CropImage;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XLabs.Ioc;
using XLabs.Platform.Services.Media;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;

namespace WineHunterX.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Inicio : ContentPage
    {
        //private readonly IMediaPicker _mediaPicker;
        //private readonly ITesseractApi _tesseract;
        //private readonly Plugin.Media.Abstractions.IMedia _mediaTaker;
        private string resultadoBarCode = null;

        public Inicio()
        {
            InitializeComponent();
            //On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);

            DesabilitarBotoes();
            HabilitarBotoes();
            //_mediaPicker = Resolver.Resolve<IMediaPicker>();
            //_tesseract = Resolver.Resolve<ITesseractApi>();
            //_mediaTaker = Resolver.Resolve<Plugin.Media.Abstractions.IMedia>();
        }

        //private async void LoadImageButton_OnClicked(object sender, EventArgs e)
        //{
        //    var result = await _mediaPicker.SelectPhotoAsync(new CameraMediaStorageOptions());
        //    await Recognise(result);
        //}

        //private async void GetPhotoButton_OnClicked(object sender, EventArgs e)
        //{
        //    XLabs.Platform.Services.Media.MediaFile result = null;
        //    try
        //    {
        //        result = await _mediaPicker.TakePhotoAsync(new CameraMediaStorageOptions());
        //    }
        //    catch (Exception err)
        //    {
        //        await DisplayAlert("Erro Camera", result + ":" + err.Message, "Cancel");
        //    }
        //    try
        //    {
        //        await Recognise(result);
        //    }
        //    catch (Exception err)
        //    {
        //        await DisplayAlert("Erro no reconhecimento da imagem.", result + ":" + err.Message, "Cancel");
        //    }

        //}

        //async Task Recognise(XLabs.Platform.Services.Media.MediaFile result)
        //{
        //    if (result.Source == null)
        //        return;

        //    var cropResult = await CropImageService.Instance.CropImage(result.Path, CropRatioType.Square);
        //    MinhaImagem.Source = cropResult.FilePath;

        //    try
        //    {
        //        activityIndicator.IsRunning = true;
        //        DesabilitarBotoes();
        //        if (!_tesseract.Initialized)
        //        {
        //            var initialised = await _tesseract.Init("por");
        //            if (!initialised)
        //                return;
        //        }
        //        //if (!await _tesseract.SetImage(result.Source))
        //        if (!await _tesseract.SetImage(cropResult.FilePath))
        //            return;
        //    }
        //    catch (Exception ex)
        //    {
        //        await DisplayAlert("Erro Tesseract",
        //            "Erro ao analisar imagem :" + ex.Message, "Cancel");
        //    }
        //    finally
        //    {
        //        activityIndicator.IsRunning = false;
        //    }
        //    TextLabel.Text = _tesseract.Text;
        //    var words = _tesseract.Results(PageIteratorLevel.Word);
        //    var symbols = _tesseract.Results(PageIteratorLevel.Symbol);
        //    var blocks = _tesseract.Results(PageIteratorLevel.Block);
        //    var paragraphs = _tesseract.Results(PageIteratorLevel.Paragraph);
        //    var lines = _tesseract.Results(PageIteratorLevel.Textline);

        //    HabilitarBotoes();
        //    await Navigation.PushAsync(new ListaInicial(TextLabel.Text));

        //}

        private void IrListaInicial(object sender, EventArgs e)
        {
            //Indicador.IsRunning = true;

            Navigation.PushAsync(new ListaInicial(""));


        }

        private async void CapturarFoto_OnClicked(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new ConsultaVinhoOCR());
            }
            catch (Exception erro)
            {
                await DisplayAlert("OCR Erro", "Erro Captura OCR" + ":" + erro.Message, "Cancel");
            }
        }

        private async void CarregarFoto_OnClicked(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new ConsultaVinhoOCR());
            }
            catch (Exception erro)
            {
                await DisplayAlert("OCR Erro", "Erro Captura OCR" + ":" + erro.Message, "Cancel");
            }
        }


        //private Xamarin.Forms.ImageSource GetImageSource(Plugin.Media.Abstractions.MediaFile m)
        //{
        //    //FileStream path = Environment.GetFolderPath(Environment.SpecialFolder).ToString();


        //    //bm = BitmapFactory.DecodeFile(path);
        //    //MemoryStream stream = new MemoryStream();
        //    //bm.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
        //    //.Compress(System.Drawing.Bitmap.CompressFormat.Png, 100, stream);
        //    byte[] Base64Stream = Convert.FromBase64String(m.ToString());
        //    return ImageSource.FromStream(() => new MemoryStream(Base64Stream));
        //    //Xamarin.Forms.Image img = ImageSource.FromStream(stream.ToArray());
        //}

        private void DesabilitarBotoes()
        {
            //GetPhotoButton.IsVisible = false;
            //LoadImageButton.IsVisible = false;
            //CapturarFoto.IsVisible = false;
            //CarregarFoto.IsVisible = false;
            //BtnScanner.IsVisible = false;
            GoogleButton.IsVisible = false;
            UserButton.IsVisible = false;
            IrListaInicialButton.IsVisible = false;
            //TelaTirarFoto.IsVisible = false;
            //TelaEscolherFoto.IsVisible = false;
            SamplePageButton.IsVisible = false;

        }
        private void HabilitarBotoes()
        {
            //GetPhotoButton.IsVisible = true;
            //LoadImageButton.IsVisible = true;
            //CapturarFoto.IsVisible = true;
            //CarregarFoto.IsVisible = true;
            //BtnScanner.IsVisible = true;
            GoogleButton.IsVisible = true;
            UserButton.IsVisible = true;
            IrListaInicialButton.IsVisible = true;
            //TelaTirarFoto.IsVisible = true;
            //TelaEscolherFoto.IsVisible = true;
            SamplePageButton.IsVisible = true;
        }

        //private async void TelaTirarFotoAction(object sender, EventArgs e)
        //{
        //    await Navigation.PushAsync(new PesquisaFoto(1));
        //}

        //private async void TelaEscolherFotoAction(object sender, EventArgs e)
        //{
        //    await Navigation.PushAsync(new PesquisaFoto(2));
        //}

        ////private void OpenScannerAsync(object sender, EventArgs e)
        ////{
        ////    //Navigation.PushAsync(new ScannerBarCode());
            
        ////    Scanner();
        ////    //ScannerView();
        ////    //resultadoBarCode = await Scanner1(true);
        ////    //Termo termo = new Termo { Descricao = resultadoBarCode };
        ////    //await Navigation.PushAsync(new ListaVinho(termo));
        ////}



        public async void Scanner()
        {
            //setup options
            var options = new MobileBarcodeScanningOptions
            {
                AutoRotate = false,
                TryHarder = false,
                //UseFrontCameraIfAvailable = false,
                DisableAutofocus = false,
                UseNativeScanning = true,
                PureBarcode=true,
                
                PossibleFormats = new List<ZXing.BarcodeFormat>
                {
                   ZXing.BarcodeFormat.EAN_8,
                   ZXing.BarcodeFormat.EAN_13
                }
            };

            var ScannerPage = new ZXingScannerPage(options)
            {
                DefaultOverlayTopText = "Alinhe a camera com o código de barras",
                DefaultOverlayBottomText = string.Empty,
                DefaultOverlayShowFlashButton = true,
                IsScanning = true, IsTorchOn = true,
                BackgroundColor = Xamarin.Forms.Color.FromHex("#853643")
            };

            ScannerPage.OnScanResult += (result) =>
            {
                // Parar de escanear
                ScannerPage.IsScanning = false;
                // Alert com o código escaneado
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PopAsync();
                    await DisplayAlert("Código escaneado", result.Text, "OK");
                    resultadoBarCode = result.Text;
                    Termo termo = new Termo { Descricao = resultadoBarCode };
                    await Navigation.PushAsync(new ListaVinho(termo));
                });
            };

            await Navigation.PushAsync(ScannerPage);
        }

        public void ScannerView()
        {
            //setup options
            var options = new MobileBarcodeScanningOptions
            {
                AutoRotate = true,
                TryHarder = false,
                //UseFrontCameraIfAvailable = false,
                DisableAutofocus = false,
                UseNativeScanning = true,
                PureBarcode = true,

                PossibleFormats = new List<ZXing.BarcodeFormat>
                {
                   ZXing.BarcodeFormat.EAN_8,
                   ZXing.BarcodeFormat.EAN_13
                }
            };

            ZXingScannerView zxing = new ZXingScannerView
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
            };

            zxing.AutoFocus();

            zxing.OnScanResult += (result) =>
            Device.BeginInvokeOnMainThread(async () =>
            {

                zxing.IsAnalyzing = false;
                zxing.IsScanning = false;

                resultadoBarCode = result.Text;
                Termo termo = new Termo { Descricao = resultadoBarCode };
                await Navigation.PushModalAsync(new ListaVinho(termo));

                //await Navigation.PushModalAsync(new ListaVinho(result.Text));

            });

            //var ScannerPage = new ZXingScannerPage(options)
            //{
            //    DefaultOverlayTopText = "Alinhe a camera com o código de barras",
            //    DefaultOverlayBottomText = string.Empty,
            //    DefaultOverlayShowFlashButton = true,
            //    IsScanning = true,
            //    IsTorchOn = true
            //};

            //ScannerPage.OnScanResult += (result) =>
            //{
            //    // Parar de escanear
            //    ScannerPage.IsScanning = false;
            //    // Alert com o código escaneado
            //    Device.BeginInvokeOnMainThread(async () =>
            //    {
            //        await Navigation.PopAsync();
            //        await DisplayAlert("Código escaneado", result.Text, "OK");
            //        resultadoBarCode = result.Text;
            //        Termo termo = new Termo { Descricao = resultadoBarCode };
            //        await Navigation.PushAsync(new ListaVinho(termo));
            //    });
            //};

            //Navigation.PushAsync(zxing);
        }

        private async Task SamplePage_OnClickedAsync(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new SamplePage());

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    //await Navigation.PushAsync(new OCRIOS());
                    await Navigation.PushAsync(new SamplePage());
                    break;
                case Device.Android:
                    await Navigation.PushAsync(new SamplePage());
                    break;
                case Device.UWP:
                    await Navigation.PushAsync(new SamplePage());
                    break;
            }
        }

        
    }
}