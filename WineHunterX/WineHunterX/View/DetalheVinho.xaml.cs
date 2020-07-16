using Plugin.Share;
using Rg.Plugins.Popup.Services;
using System;
using System.Threading.Tasks;
using WineHunterX.Armazenamento;
using WineHunterX.Model;
using WineHunterX.Servico;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WineHunterX.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DetalheVinho : ContentPage
	{
        double currentScale = 1;
        double startScale = 1;
        double xOffset = 0;
        double yOffset = 0;
        private AcessoBanco banco = new AcessoBanco();
        public VinhoLoja vl = new VinhoLoja();

		public DetalheVinho (VinhoLoja loja)
		{
			InitializeComponent ();
            //On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);

            NomeVinhoEscolhido.Text = loja.Descricao;
            LojaVinhoEscolhido.Text = loja.Loja;
            PrecoVinhoEscolhido.FormattedText = loja.Valor; //.ToString("###.##");
            LinkVinhoEscolhido.Text = loja.Link;

            FotoVinhoEscolhido.Source = loja.Imagem;

            //Indicador.IsRunning = false;
            //var img = new Image();

            var pinchGesture = new PinchGestureRecognizer();
            pinchGesture.PinchUpdated += OnPinchUpdated;
            FotoVinhoEscolhido.GestureRecognizers.Add(pinchGesture);

            //Dados do vinho encontrado
            vl = loja;

            //FotoVinhoEscolhido.AddTap(() => OnPinchUpdated(this, (Xamarin.Forms.PinchGestureUpdatedEventArgs)new EventArgs()));
            //Avalia1.AddTap(async () => await ChamarAvaliacaoActionAsync(this, new EventArgs()));
            //Avalia2.AddTap(async () => await ChamarAvaliacaoActionAsync(this, new EventArgs()));
            //Avalia3.AddTap(async () => await ChamarAvaliacaoActionAsync(this, new EventArgs()));
            //Avalia4.AddTap(async () => await ChamarAvaliacaoActionAsync(this, new EventArgs()));
            //Avalia5.AddTap(async () => await ChamarAvaliacaoActionAsync(this, new EventArgs()));
            ShareImage.AddTap(() => Share_Tapped(this, new EventArgs()));

            var TapAvalia1 = new TapGestureRecognizer();
            TapAvalia1.Tapped += async (s, e) => {
                // handle the tap
                await ChamarAvaliacaoActionAsync(this, new EventArgs());
            };
            Avalia1.GestureRecognizers.Add(TapAvalia1);

        }

        private async Task GoNavegadorAsync(object sender, EventArgs e)
        {
            //Indicador.IsRunning = true;

            if (Servico.Servico.StatusInternet() == 0)
            {
                await DisplayAlert("Alerta", "Por favor, conecte-se na internet " +
                    "para realizar a pesquisa!", "OK");
            }
            else
            {
                await Navigation.PushAsync(new WebViewLoja(LinkVinhoEscolhido.Text));
            }
            //Navigation.PushAsync(new WebViewLoja("http://www.vinhofacil.com.br"));

            
        }

        private async Task VoltarListaInicioActionAsync(object sender, EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }

        private void Share_Tapped(object sender, EventArgs e)
        {
            var ShareMessage = new Plugin.Share.Abstractions.ShareMessage
            {
                Text = NomeVinhoEscolhido.Text + "\n" +
                                         LojaVinhoEscolhido.Text + "\n" + 
                                         PrecoVinhoEscolhido.FormattedText + "\n" +
                                         LinkVinhoEscolhido.Text,
                Title = "Compartilhamento <https://br.freepik.com>",
                Url = "https://www.winehunterx.com.br"
            };

            CrossShare.Current.Share(ShareMessage);
        }

        void OnPinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
        {
            if (e.Status == GestureStatus.Started)
            {
                // Store the current scale factor applied to the wrapped user interface element,
                // and zero the components for the center point of the translate transform.
                startScale = Content.Scale;
                Content.AnchorX = 0;
                Content.AnchorY = 0;
            }
            if (e.Status == GestureStatus.Running)
            {
                // Calculate the scale factor to be applied.
                currentScale += (e.Scale - 1) * startScale;
                currentScale = Math.Max(1, currentScale);

                // The ScaleOrigin is in relative coordinates to the wrapped user interface element,
                // so get the X pixel coordinate.
                double renderedX = Content.X + xOffset;
                double deltaX = renderedX / Width;
                double deltaWidth = Width / (Content.Width * startScale);
                double originX = (e.ScaleOrigin.X - deltaX) * deltaWidth;

                // The ScaleOrigin is in relative coordinates to the wrapped user interface element,
                // so get the Y pixel coordinate.
                double renderedY = Content.Y + yOffset;
                double deltaY = renderedY / Height;
                double deltaHeight = Height / (Content.Height * startScale);
                double originY = (e.ScaleOrigin.Y - deltaY) * deltaHeight;

                // Calculate the transformed element pixel coordinates.
                double targetX = xOffset - (originX * Content.Width) * (currentScale - startScale);
                double targetY = yOffset - (originY * Content.Height) * (currentScale - startScale);

                // Apply translation based on the change in origin.
                Content.TranslationX = targetX.Clamp(-Content.Width * (currentScale - 1), 0);
                Content.TranslationY = targetY.Clamp(-Content.Height * (currentScale - 1), 0);

                // Apply scale factor.
                Content.Scale = currentScale;
            }
            if (e.Status == GestureStatus.Completed)
            {
                // Store the translation delta's of the wrapped user interface element.
                xOffset = Content.TranslationX;
                yOffset = Content.TranslationY;
            }
        }

        private async Task ChamarAvaliacaoActionAsync(object sender, EventArgs e)
        {
            await PopupNavigation.PushAsync(new PopupView(FotoVinhoEscolhido.Source, vl));
        }
    }
}