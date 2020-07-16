using AngleSharp.Parser.Html;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WineHunterX.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WineHunterX.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListaVinho : ContentPage
    {
        private List<VinhoBuscado> ListaVinhosEncontrados;
        public static readonly string GOOGLE_SEARCH_URL = "https://www.google.com/search";
        public static String searchURL = null;
        public ListaVinho(Termo termo)
        {
            InitializeComponent();

            
            PesquisaVinho.Text = termo.Descricao;
            ListaVinhosEncontrados = new List<VinhoBuscado>();

            int num = 100;

            int op = 2;

            string mensagem = termo.Descricao;

            // +searchTerm
            // pesquisa padr�o
            if (op == 1)
            {
                searchURL = GOOGLE_SEARCH_URL
                        + "?q=preço de vinho "
                        + mensagem.Trim()
                        + "&rlz=1C1CHZL_pt-BRBR748BR748&source=lnms&tbm=shop&sa=X&ved=0ahUKEwjhvqbrrrraAhVJjZAKHTiQAB8Q_AUICygC&biw=1093&bih=490"
                        + "&num=" + num;
               
                // pesquisa menor pre�o
            }
            else if (op == 2)
            {
                searchURL = GOOGLE_SEARCH_URL 
                    + "?q=" + "lojas+no+Brasil+que+vendem+bebida+vinho"
                    + "&as_oq=" + mensagem.Trim().Replace(" ","+")
                    + "&biw=1366&bih=662&tbm=shop&tbs=p_ord:p"
                    + "&ei=VXPjWpW-NcqdwASemrGoBw"
                    + "&ved=0ahUKEwjVvIT1kdvaAhXKDpAKHR5NDHUQuw0ItQIoAg"
                    + "&cr=countryBR&lr=lang_pt&gl=br&hl=br&hq=bebida" 
                    + mensagem.Trim().Replace(" ", "+")
                    + "&filter=1"
                    + "&imgSize=xxlarge"
                    + "&num=" + num;
                //+ "&dateRestrict=m[3]"
                //+ "&exactTerms=bebida"
                //+ "&excludeTerms=-Sapatos-Sapato-sapato-sapatos-Sapatilha"
                //+ "&start=1"
                //+ "&linkSite=www.wine.com.br"
                //+ "&relatedSite=www.wine.com.br"
                //+ "&siteSearch=www.wine.com.br"
                //+ "&ip=10.10.10.10&ad=w5"
                //+ "&cx=001157124722268924612:akb1vxtdzoc"

                //searchURL = GOOGLE_SEARCH_URL
                //        + "?q=preço de vinho "
                //        + mensagem.Trim()
                //        + "&biw=1366&bih=662&tbm=shop&tbs=p_ord:p&ei=VXPjWpW-NcqdwASemrGoBw&ved=0ahUKEwjVvIT1kdvaAhXKDpAKHR5NDHUQuw0ItQIoAg"
                //        + "&cr=countryBR"
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
                        + "?q=preço+de+vinho+"
                        + mensagem.Trim()
                        + "&biw=1366&bih=662&tbm=shop&tbs=p_ord:rv&ei=mXPjWoS0E4KEwQT_tL_gBg&ved=0ahUKEwjE5JiVktvaAhUCQpAKHX_aD2wQuw0IrwIoAQ"
                        + "&num=" + num;
                
            }

            BuscarTermoPesquisado(searchURL);

            //Indicador.IsRunning = false;

            //Task<List<VinhoBuscado>> vinhoB = Servico.Servico.BuscarVinhosAsync();

            //List<VinhoBuscado> ListaLocal = vinhoB.Result.ToList();
            //ListaVinhos.ItemsSource = ListaLocal;

            //TODO - Preencher a lista com resultados encontrados
            //ListaVinhos.ItemsSource = Servico.Servico.GetVinhos();

            //carregamento de imagem da internet
            //bool verifica = ImageOne.IsLoading;

            //carregamento de imagem em pasta personalizada
            //Image imgLista = new Image();
            //if (Device.RuntimePlatform == Device.UWP)
            //{
            //    vinhoB.Add(new VinhoBuscado() { Descricao = "Exemplo Vinho", Imagem = "Resources/icons8_taca_de_vinho_filled_50.png" });
            //    imgLista.Source = ImageSource.FromFile("Resources/icons8_taca_de_vinho_filled_50.png");
            //}
            //else
            //{
            //    vinhoB.Add(new VinhoBuscado() { Descricao = "Exemplo Vinho", Imagem = "icons8_taca_de_vinho_filled_50.png" });
            //    imgLista.Source = ImageSource.FromFile("icons8_taca_de_vinho_filled_50.png");
            //}

            //vinhoB.Add(new VinhoBuscado() {Descricao = "Exemplo", Imagem = imgLista.Source.ToString() });

            //foreach (VinhoBuscado vinho in ListaLocal)
            //{
            //    ListaVinhosEncontrados.Add(vinho);
            //}

            //ListaVinhos.ItemsSource = ListaVinhosEncontrados;

            //ListaVinhos.Children.Add(imgLista);
        }

        private async void BuscarTermoPesquisado(string url)
        {
            Indicador.IsRunning = true;
            ListaVinhos.IsEnabled = false;

            //ListaVinhos.ItemsSource = await Servico.Servico.MyAsyncTaskAsync(url);

            List<VinhoBuscado> retorno = await Servico.Servico.MyAsyncTaskAsync(url);
            if (!(retorno.Count > 0))
            {
                await DisplayAlert("Pesquisa de Vinhos", "Não encontramos seu vinho," +
                    "por favor, " +
                    "tente outro termo de pesquisa.", "Cancel");
            }
            else
            {
                ListaVinhos.ItemsSource = retorno;
                ListaVinhos.IsEnabled = true;
            }
            //ListaVinhos.ItemsSource = Servico.Servico.DisplayResults(conteudo);
            Indicador.IsRunning = false;
        }
        private async Task LojasActionAsync(object sender, EventArgs e)
        {
            //TODO - Implementar chamada da próxima janela de pesquisar lojas
            //VinhoBuscado vinho = (VinhoBuscado)vinhoB.ElementAt(0);

            //Indicador.IsRunning = true;

            Button b = (Button)sender;

            VinhoBuscado VinhoEncontrado = b.CommandParameter as VinhoBuscado;

            PesquisaVinho.Text = VinhoEncontrado.Descricao;

            if (Servico.Servico.StatusInternet() == 0)
            {
                await DisplayAlert("Alerta", "Por favor, conecte-se na internet " +
                    "para realizar a pesquisa!", "OK");
            }
            else
            {
                await Navigation.PushAsync(new ListaLoja(VinhoEncontrado));
            }
            //Indicador.IsRunning = false;

        }

        private async Task SelecaoVinhoActionAsync(object sender, SelectedItemChangedEventArgs e)
        {
            VinhoBuscado vinhoBuscado = (VinhoBuscado)e.SelectedItem;

            PesquisaVinho.Text = vinhoBuscado.Descricao;

            if (Servico.Servico.StatusInternet() == 0)
            {
                await DisplayAlert("Alerta", "Por favor, conecte-se na internet " +
                    "para realizar a pesquisa!", "OK");
            }
            else
            {
                await Navigation.PushAsync(new ListaLoja(vinhoBuscado));
            }
        }

        private void VoltarInicialAction(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}