using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WineHunterX.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WineHunterX.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ListaLoja : ContentPage
	{
        private List<VinhoLoja> loja;
        public static readonly string GOOGLE_SEARCH_URL = "https://www.google.com/search";
        public static String searchURL = null;

        public ListaLoja (VinhoBuscado vinho)
		{
			InitializeComponent ();

            PesquisaLoja.Text = vinho.Descricao;
            loja = new List<VinhoLoja>();

            int num = 50;

            int op = 2;

            string mensagem = vinho.Descricao;

            // +searchTerm
            // pesquisa padr�o
            if (op == 1)
            {
                searchURL = GOOGLE_SEARCH_URL
                        + "?q=venda+de+vinho "
                        + mensagem.Trim()
                        + "&rlz=1C1CHZL_pt-BRBR748BR748&source=lnms&tbm=shop&sa=X&ved=0ahUKEwjhvqbrrrraAhVJjZAKHTiQAB8Q_AUICygC&biw=1093&bih=490"
                        + "&num=" + num;

                // pesquisa menor pre�o
            }
            else if (op == 2)
            {
                searchURL = GOOGLE_SEARCH_URL
                    + "?q=" + "lojas+no+Brasil+que+vendem+bebida+vinho"
                    //+ "&as_oq=" + mensagem.Trim().Replace(" ", "+")
                    + "&biw=1366&bih=662&tbm=shop&tbs=p_ord:p"
                    + "&ei=VXPjWpW-NcqdwASemrGoBw"
                    + "&ved=0ahUKEwjVvIT1kdvaAhXKDpAKHR5NDHUQuw0ItQIoAg"
                    + "&cr=countryBR&lr=lang_pt-BR&gl=br&hl=br&hq=" + mensagem.Trim().Replace(" ", "+")
                    + "&filter=1"
                    + "&imgSize=xxlarge"
                    + "&num=" + num;

                //searchURL = GOOGLE_SEARCH_URL
                //        + "?q="
                //        + mensagem.Trim().Replace(" ","+")
                //        + "&biw=1366&bih=662&tbm=shop&tbs=p_ord:p&ei=VXPjWpW-NcqdwASemrGoBw&ved=0ahUKEwjVvIT1kdvaAhXKDpAKHR5NDHUQuw0ItQIoAg"
                //        + "&num=" + num;

                // pesquisa maior pre�o
            }
            else if (op == 3)
            {
                searchURL = GOOGLE_SEARCH_URL
                        + "?q=preço de vinho "
                        + mensagem.Trim()
                        + "&biw=1366&bih=662&tbm=shop&tbs=p_ord:pd&ei=83XjWrnmFYPEwASfy6GgCQ&ved=0ahUKEwj5q6K0lNvaAhUDIpAKHZ9lCJQQuw0IuAIoAw"
                        + "&num=" + num;

                // pesquisa melhor avalia��o
            }
            else if (op == 4)
            {
                searchURL = GOOGLE_SEARCH_URL
                        + "?q=venda+de+vinho+"
                        + mensagem.Trim()
                        + "&biw=1366&bih=662&tbm=shop&tbs=p_ord:rv&ei=mXPjWoS0E4KEwQT_tL_gBg&ved=0ahUKEwjE5JiVktvaAhUCQpAKHX_aD2wQuw0IrwIoAQ"
                        + "&num=" + num;

            }

            BuscarVinhoPesquisado(searchURL);

            //Indicador.IsRunning = false;

            //Image imgLista = new Image();
            //if (Device.RuntimePlatform == Device.UWP)
            //{
            //    loja.Add(new VinhoLoja() { Descricao = "Exemplo Vinho", Loja = "Vinho Fácil",
            //                                Imagem = "Resources/icons8_taca_de_vinho_filled_50.png",
            //                                Valor = 24.9 });
            //    imgLista.Source = ImageSource.FromFile("Resources/icons8_taca_de_vinho_filled_50.png");
            //}
            //else
            //{
            //    loja.Add(new VinhoLoja() { Descricao = "Exemplo Vinho", Loja = "Vinho Fácil",
            //                                Imagem = "icons8_taca_de_vinho_filled_50.png",
            //                                Valor = 24.9});
            //    imgLista.Source = ImageSource.FromFile("icons8_taca_de_vinho_filled_50.png");
            //}

            //ListaLojas.ItemsSource = loja;
        }

        private async void BuscarVinhoPesquisado(string url)
        {
            Indicador.IsRunning = true;
            ListaLojas.IsEnabled = false;

            List<VinhoLoja> retorno = await Servico.Servico.MyAsyncTaskAsync2(url);

            if(!(retorno.Count > 0)) { 
                await DisplayAlert("Pesquisa de Lojas", "Não encontramos lojas para este vinho , " +
                    "tente outro termo de pesquisa.", "Cancel");
            }
            else
            {
                ListaLojas.ItemsSource = retorno;
                ListaLojas.IsEnabled = true;
            }
            Indicador.IsRunning = false;
        }

        private async Task DetalheActionAsync(object sender, EventArgs e)
        {
            //Indicador.IsRunning = true;

            Button b = (Button)sender;

            VinhoLoja LojaEncontrada = b.CommandParameter as VinhoLoja;

            PesquisaLoja.Text = LojaEncontrada.Descricao;

            if (Servico.Servico.StatusInternet() == 0)
            {
                await DisplayAlert("Alerta", "Por favor, conecte-se na internet " +
                    "para realizar a pesquisa!", "OK");
            }
            else
            {
                await Navigation.PushAsync(new DetalheVinho(LojaEncontrada));
            }
            
            //VinhoLoja lojaEscolhida = (VinhoLoja)loja.ElementAt(0);

            //Navigation.PushAsync(new DetalheVinho(lojaEscolhida));
        }

        private async Task SelecaoLojaActionAsync(object sender, SelectedItemChangedEventArgs e)
        {
            VinhoLoja LojaEncontrada = (VinhoLoja)e.SelectedItem;

            PesquisaLoja.Text = LojaEncontrada.Descricao;

            if (Servico.Servico.StatusInternet() == 0)
            {
                await DisplayAlert("Alerta", "Por favor, conecte-se na internet " +
                    "para realizar a pesquisa!", "OK");
            }
            else
            {
               await Navigation.PushAsync(new DetalheVinho(LojaEncontrada));
            }
        }
    }
}