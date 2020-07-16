using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WineHunterX.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;

namespace WineHunterX.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScannerBarCode : ContentPage
    {
        ZXingScannerView zxing;
        ZXingDefaultOverlay overlay;

        public ScannerBarCode()
        {
            InitializeComponent();

            zxing = new ZXingScannerView
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
            };

            TimeSpan ts = new TimeSpan(0, 0, 0, 3, 0);
            Device.StartTimer(ts, () => {
                if (zxing.IsScanning)
                    zxing.AutoFocus();
                return true;
            });

            //zxing.AutoFocus();

            zxing.OnScanResult += (result) =>
                Device.BeginInvokeOnMainThread(async () =>
                {

                    zxing.IsAnalyzing = false;
                    zxing.IsScanning = false;

                    var resultadoBarCode = result.Text;
                    Termo termo = new Termo { Descricao = resultadoBarCode };
                    await Navigation.PushModalAsync(new ListaVinho(termo));

                });

            overlay = new ZXingDefaultOverlay
            {
                TopText = "Alinhe seu telefone com o código de barras",
                BottomText = "A leitura será realizada automaticamente",
                ShowFlashButton = zxing.HasTorch,
            };
            overlay.FlashButtonClicked += (sender, e) => {
                zxing.IsTorchOn = !zxing.IsTorchOn;
            };
            var grid = new Grid
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };
            grid.Children.Add(zxing);
            grid.Children.Add(overlay);

            // The root page of your application
            Content = grid;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            zxing.IsScanning = true;
        }

        protected override void OnDisappearing()
        {
            zxing.IsScanning = false;

            base.OnDisappearing();
        }
    }

        //private CancellationTokenSource cancelTimer;

        //async public Task<string> Scanner(bool flashOn)
        //{
        //    this.cancelTimer = new CancellationTokenSource();

        //    var scanner = new MobileBarcodeScanner();
        //    scanner.BottomText = "Ensure the barcode is upright and inside the viewfinder.";

        //    ZXing.Result result = null;

        //    CancellationTokenSource cts = this.cancelTimer;
        //    TimeSpan ts = new TimeSpan(0, 0, 0, 3, 0);
        //    Device.StartTimer(ts, () =>
        //    {
        //        if (cts.IsCancellationRequested)
        //        {
        //            return false;
        //        }

        //        if (result == null)
        //        {
        //            scanner.AutoFocus();
        //            if (flashOn)
        //            {
        //                scanner.Torch(true);
        //            }
        //            return true;
        //        }
        //        return false;
        //    });

        //    result = await scanner.Scan();

        //    if (result != null)
        //    {
        //        await Stop();
        //        return result.Text;
        //    }

        //    await Stop();
        //    return string.Empty;
        //}

        //async private Task Stop()
        //{
        //    await Task.Run(() => { Interlocked.Exchange(ref this.cancelTimer, new CancellationTokenSource()).Cancel(); });
        //}
    
}