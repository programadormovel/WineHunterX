using Rg.Plugins.Popup.Services;
using System;
using System.Linq;
using WineHunterX.Armazenamento;
using WineHunterX.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WineHunterX.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PopupView
	{
        private AcessoBanco banco = new AcessoBanco();
        private double valorAvaliacao = 0.0;

        public PopupView(ImageSource ImagemVinhoAmpliada, VinhoLoja vinhoLojaAvaliado)
        {
            InitializeComponent();

            var img = new Image()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Color.White,
                Aspect = Aspect.AspectFit,
                MinimumHeightRequest = 200,
                MinimumWidthRequest = 200
            };

            var TextoClassifica = new Label()
            {
                Text = "Clique nas taças para classificar este vinho:",
                FontSize = 16
            };

            var TxtTacinha1 = new Label()
            {
                Text = "Ruim",
                TextColor = Color.Red,
                FontSize = 10,
                HorizontalTextAlignment = TextAlignment.Center
            };

            var TxtTacinha2 = new Label()
            {
                Text = "Regular",
                TextColor = Color.DeepPink,
                FontSize = 10,
                HorizontalTextAlignment = TextAlignment.Center
            };
            var TxtTacinha3 = new Label()
            {
                Text = "Bom",
                TextColor = Color.Orange,
                FontSize = 10,
                HorizontalTextAlignment = TextAlignment.Center
            };
            var TxtTacinha4 = new Label()
            {
                Text = "Ótimo",
                TextColor = Color.Green,
                FontSize = 10,
                HorizontalTextAlignment = TextAlignment.Center
            };
            var TxtTacinha5 = new Label()
            {
                Text = "Excelente",
                TextColor = Color.DarkGreen,
                FontSize = 10,
                HorizontalTextAlignment = TextAlignment.Center
            };
            var tacinha1 = new Image()
            {
                Source = "tacinhav3.png",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                BackgroundColor = Color.Transparent
            };
            var tacinha2 = new Image()
            {
                Source = "tacinhav3.png",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                BackgroundColor = Color.Transparent
            };
            var tacinha3 = new Image()
            {
                Source = "tacinhav3.png",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                BackgroundColor = Color.Transparent
            };
            var tacinha4 = new Image()
            {
                Source = "tacinhav3.png",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                BackgroundColor = Color.Transparent
            };
            var tacinha5 = new Image()
            {
                Source = "tacinhav3.png",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                BackgroundColor = Color.Transparent
            };

            StackLayout stackTacinha1 = new StackLayout()
            {
                Children = { tacinha1, TxtTacinha1 },
                WidthRequest = 50,
                Padding = 2,
                BackgroundColor = Color.Transparent,
                HorizontalOptions = LayoutOptions.Center,
                Orientation = StackOrientation.Vertical
            };
            StackLayout stackTacinha2 = new StackLayout()
            {
                Children = { tacinha2, TxtTacinha2 },
                WidthRequest = 50,
                Padding = 2,
                BackgroundColor = Color.Transparent,
                HorizontalOptions = LayoutOptions.Center,
                Orientation = StackOrientation.Vertical
            };
            StackLayout stackTacinha3 = new StackLayout()
            {
                Children = { tacinha3, TxtTacinha3 },
                WidthRequest = 50,
                Padding = 2,
                BackgroundColor = Color.Transparent,
                HorizontalOptions = LayoutOptions.Center,
                Orientation = StackOrientation.Vertical
            };
            StackLayout stackTacinha4 = new StackLayout()
            {
                Children = { tacinha4, TxtTacinha4 },
                WidthRequest = 50,
                Padding = 2,
                BackgroundColor = Color.Transparent,
                HorizontalOptions = LayoutOptions.Center,
                Orientation = StackOrientation.Vertical
            };
            StackLayout stackTacinha5 = new StackLayout()
            {
                Children = { tacinha5, TxtTacinha5 },
                WidthRequest = 80,
                Padding = 2,
                BackgroundColor = Color.Transparent,
                HorizontalOptions = LayoutOptions.Center,
                Orientation = StackOrientation.Vertical
            };

            var ok = new Button()
            {
                Text = "OK",
                TextColor = Color.White,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Center,
                BackgroundColor = Color.MediumVioletRed
            };

            var TituloComentario = new Label()
            {
                Text = "Deixe abaixo seu comentário sobre este vinho:",
                FontSize = 14
            };

            var pesqVinho = banco.ConsultarVinhoLoja();
            var textoVinho = "";
            double avaliacao = 0.0;
            var res = pesqVinho.Where(a => a.Descricao.Contains(vinhoLojaAvaliado.Descricao)).ToList();

            if (res.Count > 0)
            {
                textoVinho = res.ElementAt<VinhoLoja>(0).Opiniao.ToString();
                avaliacao = res.ElementAt<VinhoLoja>(0).Avaliacao;
                if(avaliacao>=1.0 && avaliacao<2.0){
                    tacinha2.Source = "tacinhav3.png";
                    tacinha3.Source = "tacinhav3.png";
                    tacinha4.Source = "tacinhav3.png";
                    tacinha5.Source = "tacinhav3.png";
                    tacinha1.Source = "tacinha.png";
                    valorAvaliacao = 1.0;
                }else if(avaliacao >= 2.0 && avaliacao < 3.0){
                    tacinha2.Source = "tacinha.png";
                    tacinha3.Source = "tacinhav3.png";
                    tacinha4.Source = "tacinhav3.png";
                    tacinha5.Source = "tacinhav3.png";
                    tacinha1.Source = "tacinha.png";
                    valorAvaliacao = 1.0;
                }else if (avaliacao >= 3.0 && avaliacao < 4.0)
                {
                    tacinha2.Source = "tacinha.png";
                    tacinha3.Source = "tacinha.png";
                    tacinha4.Source = "tacinhav3.png";
                    tacinha5.Source = "tacinhav3.png";
                    tacinha1.Source = "tacinha.png";
                    valorAvaliacao = 1.0;
                }else if (avaliacao >= 4.0 && avaliacao < 5.0)
                {
                    tacinha2.Source = "tacinha.png";
                    tacinha3.Source = "tacinha.png";
                    tacinha4.Source = "tacinha.png";
                    tacinha5.Source = "tacinhav3.png";
                    tacinha1.Source = "tacinha.png";
                    valorAvaliacao = 1.0;
                }else if (avaliacao >= 5.0 && avaliacao <= 5.0)
                {
                    tacinha2.Source = "tacinha.png";
                    tacinha3.Source = "tacinha.png";
                    tacinha4.Source = "tacinha.png";
                    tacinha5.Source = "tacinha.png";
                    tacinha1.Source = "tacinha.png";
                    valorAvaliacao = 1.0;
                }
            }
            else
            {
                textoVinho = "";
            }

            var TextoComentario = new Editor()
            {
                Text = textoVinho,
                MinimumHeightRequest = 400,
                MinimumWidthRequest = 100,
                MaxLength=254,
                BackgroundColor = Color.White
            };

            var FrameComentario = new Frame()
            {
                Content = TextoComentario,
                HasShadow = false,
                BorderColor = Color.Black,
                CornerRadius = 1,
                Padding = 10,
                HeightRequest = 150,
                MinimumWidthRequest = 350,
                BackgroundColor = Color.White
            };

            ok.Clicked += (object sender, EventArgs e) =>
            {
                //Grava o vinho avaliado e sua respectiva avaliação
                VinhoLoja vinhoLoja = vinhoLojaAvaliado;
                bool gravaVinhoLoja = false;
                //Consulta o termo e grava se não existir
                var pesq = banco.ConsultarVinhoLoja();
                var listaVL = pesq.Where(a => a.Descricao.Contains(vinhoLojaAvaliado.Descricao)).ToList();

                if (listaVL.Count > 0)
                {
                    gravaVinhoLoja = false;
                }
                else
                {
                    gravaVinhoLoja = true;
                }

                if (gravaVinhoLoja == true)
                {
                    vinhoLoja.Opiniao = TextoComentario.Text;
                    vinhoLoja.Avaliacao = valorAvaliacao;
                    banco.CadastroVinhoLoja(vinhoLoja);
                }

                PopupNavigation.PopAsync();
            };

            //switch (Device.RuntimePlatform)
            //{
            //    case Device.iOS:
            //        Padding = new Thickness(0, 20, 0, 0);
            //        tacinha1.Source = "tacinhav3.png";
            //        tacinha2.Source = "tacinhav3.png";
            //        tacinha3.Source = "tacinhav3.png";
            //        tacinha4.Source = "tacinhav3.png";
            //        tacinha5.Source = "tacinhav3.png";
            //        break;
            //    case Device.Android:
            //        Padding = new Thickness(0, 0, 0, 0);
            //        tacinha1.Source = "tacinhav3.png";
            //        tacinha2.Source = "tacinhav3.png";
            //        tacinha3.Source = "tacinhav3.png";
            //        tacinha4.Source = "tacinhav3.png";
            //        tacinha5.Source = "tacinhav3.png";
            //        break;
            //    case Device.UWP:
            //        Padding = new Thickness(0, 0, 0, 0);
            //        tacinha1.Source = "Resources/tacinhav3.png";
            //        tacinha2.Source = "Resources/tacinhav3.png";
            //        tacinha3.Source = "Resources/tacinhav3.png";
            //        tacinha4.Source = "Resources/tacinhav3.png";
            //        tacinha5.Source = "Resources/tacinhav3.png";
            //        break;
            //}

            StackLayout stackTacas = new StackLayout()
            {
                Children = { stackTacinha1, stackTacinha2, 
                    stackTacinha3, stackTacinha4, stackTacinha5 },
                BackgroundColor = Color.Transparent,
                HorizontalOptions = LayoutOptions.Center,
                Orientation = StackOrientation.Horizontal,
                Margin = 0, Spacing = 0
            };

            //StackLayout stackAvaliacao = new StackLayout()
            //{
            //    Children = { TxtTacinha1, TxtTacinha2, TxtTacinha3, TxtTacinha4, TxtTacinha5 },
            //    BackgroundColor = Color.Transparent,
            //    HorizontalOptions = LayoutOptions.Center,
            //    Orientation = StackOrientation.Horizontal
            //};

            StackLayout stack = new StackLayout()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Children = {TextoClassifica, img, stackTacas, TituloComentario, FrameComentario, ok},
                BackgroundColor = Color.White
            };

            ScrollView scroll = new ScrollView()
            {
                Content = stack
            };

            Content = scroll;
            //Padding = new Thickness(0, Device.OnPlatform(20, 0, 0), 0, 0);

            img.Source = ImagemVinhoAmpliada;

            //Ruim
            var tapGestureRecognizer1 = new TapGestureRecognizer();
            tapGestureRecognizer1.Tapped += (s, e) => {
                // handle the tap
                tacinha2.Source = "tacinhav3.png";
                tacinha3.Source = "tacinhav3.png";
                tacinha4.Source = "tacinhav3.png";
                tacinha5.Source = "tacinhav3.png";
                tacinha1.Source = "tacinha.png";
                valorAvaliacao = 1.0;
            };
            tacinha1.GestureRecognizers.Add(tapGestureRecognizer1);

            var tapGestureRecognizer2 = new TapGestureRecognizer();
            tapGestureRecognizer2.Tapped += (s, e) => {
                // handle the tap
                tacinha1.Source = "tacinha.png";
                tacinha2.Source = "tacinha.png";
                tacinha3.Source = "tacinhav3.png";
                tacinha4.Source = "tacinhav3.png";
                tacinha5.Source = "tacinhav3.png";
                valorAvaliacao = 2.0;
            };
            tacinha2.GestureRecognizers.Add(tapGestureRecognizer2);

            var tapGestureRecognizer3 = new TapGestureRecognizer();
            tapGestureRecognizer3.Tapped += (s, e) => {
                // handle the tap
                tacinha1.Source = "tacinha.png";
                tacinha2.Source = "tacinha.png";
                tacinha3.Source = "tacinha.png";
                tacinha4.Source = "tacinhav3.png";
                tacinha5.Source = "tacinhav3.png";
                valorAvaliacao = 3.0;
            };
            tacinha3.GestureRecognizers.Add(tapGestureRecognizer3);

            var tapGestureRecognizer4 = new TapGestureRecognizer();
            tapGestureRecognizer4.Tapped += (s, e) => {
                // handle the tap
                tacinha1.Source = "tacinha.png";
                tacinha2.Source = "tacinha.png";
                tacinha3.Source = "tacinha.png";
                tacinha4.Source = "tacinha.png";
                tacinha5.Source = "tacinhav3.png";
                valorAvaliacao = 4.0;
            };
            tacinha4.GestureRecognizers.Add(tapGestureRecognizer4);

            var tapGestureRecognizer5 = new TapGestureRecognizer();
            tapGestureRecognizer5.Tapped += (s, e) => {
                // handle the tap
                tacinha1.Source = "tacinha.png";
                tacinha2.Source = "tacinha.png";
                tacinha3.Source = "tacinha.png";
                tacinha4.Source = "tacinha.png";
                tacinha5.Source = "tacinha.png";
                valorAvaliacao = 5.0;
            };
            tacinha5.GestureRecognizers.Add(tapGestureRecognizer5);

        }
	}
}