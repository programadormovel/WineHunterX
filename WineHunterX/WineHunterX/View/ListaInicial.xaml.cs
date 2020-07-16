using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WineHunterX.Armazenamento;
using WineHunterX.Model;
using WineHunterX.Servico;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;

namespace WineHunterX.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListaInicial : ContentPage
    {
        private List<Termo> lista = null;
        private AcessoBanco banco = new AcessoBanco();
        private string resultadoBarCode = null;


        public ListaInicial(string textoCapturado)
        {
            InitializeComponent();

            if (!textoCapturado.Equals(""))
                PesquisaTermo.Text = textoCapturado;


            lista = new List<Termo>();

            lista = banco.Consultar();

            if (lista.Count <= 0)
            {
                lista.Add(new Termo { Descricao = "Merlot" });
                lista.Add(new Termo { Descricao = "Cabernet" });
                lista.Add(new Termo { Descricao = "Branco" });
                lista.Add(new Termo { Descricao = "Tinto" });
                lista.Add(new Termo { Descricao = "Tannat" });
                lista.Add(new Termo { Descricao = "Seco" });
            }

            ListaTermos.ItemsSource = lista;

            //Indicador.IsRunning = false;
        }

        private void ChamadaDireta(object obj)
        {
            PesquisarButtonAsync(obj, new EventArgs());
        }

        private void PesquisarButtonAsync(object sender, EventArgs e)
        {
            //Indicador.IsRunning = true;
            string textoTermo = PesquisaTermo.Text;
            Termo termo = new Termo { Descricao = textoTermo };
            bool gravaTermo = false;

            //Consulta o termo e grava se não existir
            var pesq = banco.Consultar();
            lista = pesq.Where(a => a.Descricao.ToLower().Contains(e.ToString().ToLower())).ToList();

            if (lista.Count > 0)
            {
                foreach (Termo t in lista)
                {
                    if (t.Descricao.ToLower().Equals(termo.Descricao.ToLower()))
                        gravaTermo = false;
                    else
                        gravaTermo = true;
                }
            }
            else
            {
                gravaTermo = true;
            }

            if (gravaTermo == true)
            {
                banco.Cadastro(termo);
                lista.Add(termo);
                ListaTermos.ItemsSource = lista;
            }

            ChamarTelaListaVinhoAsync(termo);
        }

        private void SelecaoTermoActionAsync(object sender, SelectedItemChangedEventArgs e)
        {
            //Indicador.IsRunning = true;

            Termo termo = (Termo)e.SelectedItem;

            PesquisaTermo.Text = termo.Descricao;

            bool gravaTermo = false;

            var pesq = banco.Consultar();

            if (pesq.Count > 0)
            {
                foreach (Termo t in pesq)
                {
                    if (t.Descricao.ToLower().Equals(termo.Descricao.ToLower()))
                        gravaTermo = false;
                    else
                        gravaTermo = true;
                }
            }
            else
            {
                gravaTermo = true;
            }

            if (gravaTermo==true) banco.Cadastro(termo);

            ChamarTelaListaVinhoAsync(termo);


        }

        private void ChamarTelaListaVinhoAsync(Termo termo)
        {
            if (Servico.Servico.StatusInternet() == 0)
            {
                DisplayAlert("Alerta", "Por favor, conecte-se na internet " +
                    "para realizar a pesquisa!", "OK");
            }
            else
            {
                Navigation.PushAsync(new ListaVinho(termo));
            }
        }

        //private async void ScannerButtonAsync(object sender, EventArgs e)
        //{
        //    //setup options
        //    var options = new MobileBarcodeScanningOptions
        //    {
        //        AutoRotate = false,
        //        TryHarder = false,
        //        //UseFrontCameraIfAvailable = false,
        //        DisableAutofocus = false,
        //        UseNativeScanning = true,
        //        PureBarcode = true,

        //        PossibleFormats = new List<ZXing.BarcodeFormat>
        //        {
        //           ZXing.BarcodeFormat.EAN_8,
        //           ZXing.BarcodeFormat.EAN_13
        //        }
        //    };

        //    var ScannerPage = new ZXingScannerPage(options)
        //    {
        //        DefaultOverlayTopText = "Alinhe a camera com o código de barras",
        //        DefaultOverlayBottomText = string.Empty,
        //        DefaultOverlayShowFlashButton = true,
        //        IsScanning = true,
        //        IsTorchOn = true
        //    };

        //    ScannerPage.OnScanResult += (result) =>
        //    {
        //        // Parar de escanear
        //        ScannerPage.IsScanning = false;
        //        // Alert com o código escaneado
        //        Device.BeginInvokeOnMainThread(async () =>
        //        {
        //            await Navigation.PopAsync();
        //            await DisplayAlert("Código escaneado", result.Text, "OK");
        //            resultadoBarCode = result.Text;
        //            Termo termo = new Termo { Descricao = resultadoBarCode };
        //            await Navigation.PushAsync(new ListaVinho(termo));
        //        });
        //    };

        //    await Navigation.PushAsync(ScannerPage);

        //}

        private async void FotoButtonAsync(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new SamplePage());

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    await Navigation.PushAsync(new ComputerVision());
                    break;
                case Device.Android:
                    await Navigation.PushAsync(new ComputerVision());
                    break;
                case Device.UWP:
                    await Navigation.PushAsync(new ComputerVision());
                    break;
            }
        }
    }
}






