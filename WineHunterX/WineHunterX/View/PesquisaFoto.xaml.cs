using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract;
using Xamarians.CropImage;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XLabs.Ioc;
using XLabs.Platform.Services.Media;

namespace WineHunterX.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PesquisaFoto : ContentPage
    {
        private readonly IMediaPicker _mediaPicker;
        private readonly ITesseractApi _tesseract;
        private readonly Plugin.Media.Abstractions.IMedia _mediaTaker;
        //private readonly IVisionServiceClient _visionServiceClient;

        public PesquisaFoto(int i)
        {
            InitializeComponent();
            _mediaPicker = Resolver.Resolve<IMediaPicker>();
            _tesseract = Resolver.Resolve<ITesseractApi>();
            _mediaTaker = Resolver.Resolve<Plugin.Media.Abstractions.IMedia>();

            //_visionServiceClient = new VisionServiceClient("2bdc5069dafd4c27877438d4328430c4");

            //Indicador.IsRunning = false;

            if (i == 1)
            {
                TirarFotoAction(this, new EventArgs());
            }
            else if (i == 2)
            {
                EscolherFotoAction(this, new EventArgs());
            }
        }

        private async void LoadImageButton_OnClicked(object sender, EventArgs e)
        {
            var result = await _mediaPicker.SelectPhotoAsync(new CameraMediaStorageOptions());
            await Recognise(result);
        }

        private async void GetPhotoButton_OnClicked(object sender, EventArgs e)
        {
            XLabs.Platform.Services.Media.MediaFile result = null;
            try
            {
                result = await _mediaPicker.TakePhotoAsync(new CameraMediaStorageOptions());
            }
            catch (Exception err)
            {
                await DisplayAlert("Erro Camera", result + ":" + err.Message, "Cancel");
            }
            try
            {
                await Recognise(result);
            }
            catch (Exception err)
            {
                await DisplayAlert("Erro no reconhecimento da imagem.", result + ":" + err.Message, "Cancel");
            }

        }

        async Task Recognise(XLabs.Platform.Services.Media.MediaFile result)
        {
            if (result.Source == null)
                return;
            try
            {
                activityIndicator.IsRunning = true;

                if (!_tesseract.Initialized)
                {
                    var initialised = await _tesseract.Init("por+eng");
                    if (!initialised)
                        return;
                }
                if (!await _tesseract.SetImage(result.Source))
                    return;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro Tesseract",
                    "Erro ao analisar imagem :" + ex.Message, "Cancel");
            }
            finally
            {
                activityIndicator.IsRunning = false;
            }
            TextLabel.Text = _tesseract.Text;
            var words = _tesseract.Results(PageIteratorLevel.Word);
            var symbols = _tesseract.Results(PageIteratorLevel.Symbol);
            var blocks = _tesseract.Results(PageIteratorLevel.Block);
            var paragraphs = _tesseract.Results(PageIteratorLevel.Paragraph);
            var lines = _tesseract.Results(PageIteratorLevel.Textline);


            await Navigation.PushAsync(new ListaInicial(TextLabel.Text));

        }

        private void IrListaInicial(object sender, EventArgs e)
        {
            //Indicador.IsRunning = true;

            Navigation.PushAsync(new ListaInicial(""));


        }

        private async void CapturarFoto_OnClicked(object sender, EventArgs e)
        {
            Plugin.Media.Abstractions.MediaFile result = null;
            try
            {
                result =
                    await _mediaTaker.TakePhotoAsync(
                        new StoreCameraMediaOptions());
            }
            catch (Exception erro)
            {
                await DisplayAlert("Erro Camera", result + ":" + erro.Message, "Cancel");
            }
            try
            {
                await Recognise2(result);
            }
            catch (Exception erro)
            {
                await DisplayAlert("Erro no reconhecimento da imagem.", result + ":" + erro.Message, "Cancel");
            }
        }

        private async void CarregarFoto_OnClicked(object sender, EventArgs e)
        {
            Plugin.Media.Abstractions.MediaFile result = null;
            try
            {
                result = await _mediaTaker.PickPhotoAsync(
                 new Plugin.Media.Abstractions.PickMediaOptions());
                //await Recognise2(result);
            }
            catch (Exception erro)
            {
                await DisplayAlert("Erro Camera", erro.Message, "Cancel");
            }
            try
            {
                await Recognise2(result);
            }
            catch (Exception erro)
            {
                await DisplayAlert("Erro no reconhecimento da imagem.", result + ":" + erro.Message, "Cancel");
            }

        }

        async Task Recognise2(Plugin.Media.Abstractions.MediaFile result)
        {
            if (result == null)
                return;
            try
            {
                activityIndicator.IsRunning = true;
                if (!_tesseract.Initialized)
                {
                    var initialised = await _tesseract.Init("por+eng");
                    if (!initialised)
                        return;
                }
                if (!await _tesseract.SetImage(result.Path))
                    return;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro Tesseract",
                    "Erro ao analisar imagem :" + ex.Message, "Cancel");
            }
            finally
            {
                activityIndicator.IsRunning = false;
            }
            TextLabel.Text = _tesseract.Text;
            var words = _tesseract.Results(PageIteratorLevel.Word);
            var symbols = _tesseract.Results(PageIteratorLevel.Symbol);
            var blocks = _tesseract.Results(PageIteratorLevel.Block);
            var paragraphs = _tesseract.Results(PageIteratorLevel.Paragraph);
            var lines = _tesseract.Results(PageIteratorLevel.Textline);

            await Navigation.PushAsync(new ListaInicial(TextLabel.Text));

        }

        //private async Task<string> reconhecerCaptchaAsync(Xamarin.Forms.Image img)
        //{
        //    //Bitmap imagem = new Bitmap(img);
        //    System.Drawing.Bitmap imagem = await GetBitmap(img);
        //    imagem = imagem.Clone(new System.Drawing.Rectangle(0, 0, img.Width, img.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
        //    Erosion erosion = new Erosion();
        //    Dilatation dilatation = new Dilatation();
        //    Invert inverter = new Invert();
        //    ColorFiltering cor = new ColorFiltering();
        //    cor.Blue = new AForge.IntRange(200, 255);
        //    cor.Red = new AForge.IntRange(200, 255);
        //    cor.Green = new AForge.IntRange(200, 255);
        //    Opening open = new Opening();
        //    BlobsFiltering bc = new BlobsFiltering();
        //    Closing close = new Closing();
        //    GaussianSharpen gs = new GaussianSharpen();
        //    ContrastCorrection cc = new ContrastCorrection();
        //    bc.MinHeight = 10;
        //    FiltersSequence seq = new FiltersSequence(gs, inverter, open, inverter, bc, inverter, open, cc, cor, bc, inverter);
        //    pictureBox.Source = await GetImageSource(seq.Apply(imagem));
        //    string reconhecido = OCR(await GetBitmap(pictureBox));
        //    return reconhecido;
        //}

        //private string OCR(System.Drawing.Bitmap b)
        //{
        //    string res = "";
        //    using (var engine = new TesseractEngine(@"tessdata", "por", OcrEngineMode.TesseractOnly))
        //    {
        //        engine.SetVariable("tessedit_char_whitelist", "1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ");
        //        engine.SetVariable("tessedit_unrej_any_wd", true);

        //        using (var page = engine.Process(b, PageSegMode.SingleLine))
        //            res = page.GetText();
        //    }
        //    return res;
        //}

        //private async Task<System.Drawing.Bitmap> GetBitmap(Xamarin.Forms.Image image)
        //{

        //}

        private Xamarin.Forms.ImageSource GetImageSource(Plugin.Media.Abstractions.MediaFile m)
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




        private async void TirarFotoAction(object sender, EventArgs e)
        {
            try
            {
                await CrossMedia.Current.Initialize();

                activityIndicator.IsRunning = true;

                if (!CrossMedia.Current.IsTakePhotoSupported || !CrossMedia.Current.IsCameraAvailable)
                {
                    await DisplayAlert("Ops", "Nenhuma câmera detectada.", "OK");

                    activityIndicator.IsRunning = false;

                    return;
                }

                var file = await CrossMedia.Current.TakePhotoAsync(
                    new StoreCameraMediaOptions
                    {
                        //SaveToAlbum = true,
                        Directory = "Pictures",
                        //AllowCropping = true,
                        //RotateImage = true,
                        Name = "OCRimage"
                    });

                if (file == null)
                {
                    activityIndicator.IsRunning = false;
                    return;
                }

                //var cropResult = await CropImageService.Instance.CropImage(file.Path, CropRatioType.None);

                //MinhaImagem.Source = cropResult.FilePath;



                MinhaImagem.Source = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();

                    return stream;

                });

                //await GetHandwriting(url);

                //Reconhecer caracteres
                var stream1 = file.GetStream();


                byte[] fileBytes = null;
                using (var memoryStream = new MemoryStream())
                {
                    stream1.CopyTo(memoryStream);
                    //stream1.Dispose();
                    //file.Dispose();
                    fileBytes = memoryStream.ToArray();
                }

                activityIndicator.IsRunning = false;

                await GetTextRecognized(file.GetStream());

                //activityIndicator.IsRunning = false;

                //await takeContinuousPhotos(fileBytes);
            }
            catch (Exception erro)
            {
                await DisplayAlert("Erro Tirar Foto", "Erro na captura da foto: " +
                    erro.Message, "Cancel");
            }
            finally
            {
                //stream1.Dispose();
                //file.Dispose();
            }
        }

        private async void EscolherFotoAction(object sender, EventArgs e)
        {
            try
            {
                await CrossMedia.Current.Initialize();

                activityIndicator.IsRunning = true;

                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    await DisplayAlert("Ops", "Galeria de fotos não suportada.", "OK");

                    activityIndicator.IsRunning = false;

                    return;
                }

                var file = await CrossMedia.Current.PickPhotoAsync();

                //var cropResult = await CropImageService.Instance.CropImage(file.Path, CropRatioType.None);

                //file = cropResult.FilePath;

                if (file == null)
                {
                    activityIndicator.IsRunning = false;
                    return;
                }

                //MinhaImagem.Source = cropResult.FilePath;





                MinhaImagem.Source = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();

                    return stream;

                });



                //await GetTextRecognized(url);

                //Reconhecer caracteres
                var stream1 = file.GetStream();
                //var stream1 = cropResult;


                byte[] fileBytes = null;
                using (var memoryStream = new MemoryStream())
                {
                    stream1.CopyTo(memoryStream);
                    //stream1.Dispose();

                    fileBytes = memoryStream.ToArray();
                }

                activityIndicator.IsRunning = false;

                await GetTextRecognized(file.GetStream());

                //await takeContinuousPhotos(fileBytes);

            }
            catch (Exception erro)
            {
                await DisplayAlert("Erro Escolher Foto", "Erro na escolha da foto: " +
                    erro.Message, "Cancel");
            }
            finally
            {
                //stream1.Dispose();
                //file.Dispose();
            }



        }

        private async Task GetTextRecognized(Stream caminhoImagem)
        {
            activityIndicator.IsRunning = true;

            //HandwritingRecognitionOperation operation = new HandwritingRecognitionOperation();
            //operation.Url = texto;
            TextLabel.Text = "";

            //try
            //{
            //    //var result = await _visionServiceClient
            //    //    .RecognizeTextAsync(caminhoImagem);
            //    //.GetHandwritingRecognitionOperationResultAsync(operation);


            //    //TextLabel.Text += result.ToString() + " ";
            //}
            //catch (Exception c)
            //{
            //    await DisplayAlert("Erro Reconhecimento Visual Microsoft", c.Message, "Cancel");
            //}
            //catch (Exception e)
            //{
            //    await DisplayAlert("Error", e.Message, "ok");
            //}
            //finally
            //{
            //    activityIndicator.IsRunning = false;
            //}
        }

        private async Task CreateHandwriting(string image)
        {
            //try
            //{
            //    using (Stream imageFileStream = File.OpenRead(image))
            //    {
            //        var result = await _visionServiceClient.CreateHandwritingRecognitionOperationAsync(imageFileStream);
            //        url = result.Url;
            //    }
            //}
            //catch (ClientException c)
            //{
            //    await DisplayAlert("Erro Camera Microsoft", c.Message, "Cancel");
            //}
            //catch (Exception e)
            //{
            //    await DisplayAlert("Error", e.Message, "ok");
            //}
        }

        public async Task takeContinuousPhotos(byte[] bytes)
        {

            //byte[] newBytes;
            //byte[] bytes;            
            //bytes = stream.;

            //newBytes = this.CropPhoto(bytes, new System.Drawing.Rectangle(40, view.Height / 2 - 200, view.Width - 40, 100), (view.Width - 40) * 2, 200);

            try
            {
                if (!_tesseract.Initialized)
                {
                    activityIndicator.IsRunning = true;

                    await _tesseract.Init("por+eng");

                    var result = await _tesseract.SetImage(new MemoryStream(bytes));

                    if (result)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            this.TextLabel.Text = this._tesseract.Text;
                        });
                        _tesseract.Clear();
                        bytes = new byte[0];
                        //newBytes = new byte[0];
                    }
                }
                else
                {
                    var result = await _tesseract.SetImage(new MemoryStream(bytes));
                    //var result = await _tesseract.SetImage(new MemoryStream(newBytes));
                    if (result)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            this.TextLabel.Text = this._tesseract.Text;
                        });
                        _tesseract.Clear();
                        bytes = new byte[0];
                        //newBytes = new byte[0];
                    }
                }

            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro Tesseract OCR",
                    "Erro ao analisar imagem :" + ex.Message, "Cancel");
            }
            finally
            {
                activityIndicator.IsRunning = false;
            }


            await Navigation.PushAsync(new ListaInicial(TextLabel.Text));
        }
    }
}