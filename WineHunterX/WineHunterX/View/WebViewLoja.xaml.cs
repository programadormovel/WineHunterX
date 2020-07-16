using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WineHunterX.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class WebViewLoja : ContentPage
    {
		public WebViewLoja (string caminho)
		{
			InitializeComponent ();

            EnderecoSite.Text = caminho.Replace("http://www.google","https://www.google");

            GoPagina(this, new EventArgs());

                       
        }

        public void GoPagina(object sender, EventArgs args)
        {
            Navegador.Source = EnderecoSite.Text;
        }

        public void GoProximo(object sender, EventArgs args)
        {
            if (Navegador.CanGoForward)
            {
                Navegador.GoForward();
            }
        }

        public void GoVoltar(object sender, EventArgs args)
        {
            if (Navegador.CanGoBack)
            {
                Navegador.GoBack();
            }
        }

        public void ActionCarregando(object sender, EventArgs args)
        {
            LblStatus.Text = "Carregando...";
        }

        public void ActionCarregado(object sender, EventArgs args)
        {
            LblStatus.Text = "Finalizado.";
        }

        private async Task VoltarListaInicioActionAsync(object sender, EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }

    }
}